import socket
import json
import threading
import math
import queue
from collections import defaultdict

# Global variables
house_coordinates = defaultdict(dict)
consumption_data = defaultdict(float)
suspicion = {}
ready_signal_received = False
lock = threading.Lock()
LOW_CONSUMPTION_THRESHOLD = 30
SUSPICION_INCREASE_AMOUNT_LOW = 1
SUSPICION_INCREASE_AMOUNT_NEIGHBOUR = 10
NORMAL_CONSUMPTION_MAX = 50

def calculate_distance(lat1, lon1, lat2, lon2):
    # Convert latitude and longitude from degrees to radians
    lat1_rad = math.radians(lat1)
    lon1_rad = math.radians(lon1)
    lat2_rad = math.radians(lat2)
    lon2_rad = math.radians(lon2)

    # Earth radius in kilometers
    earth_radius = 6371.0

    # Calculate the differences
    delta_lat = lat2_rad - lat1_rad
    delta_lon = lon2_rad - lon1_rad

    # Haversine formula to compute distance
    a = math.sin(delta_lat / 2) ** 2 + math.cos(lat1_rad) * math.cos(lat2_rad) * math.sin(delta_lon / 2) ** 2
    c = 2 * math.atan2(math.sqrt(a), math.sqrt(1 - a))
    distance = earth_radius * c

    return distance

MAX_DISTANCE = 0.1

def find_neighboring_houses(house_id, house_coordinates):
    neighboring_houses = []
    house_lat, house_long = house_coordinates[house_id]
    for neighbor_id, neighbor_coordinates in house_coordinates.items():
        if neighbor_id != house_id:
            neighbor_lat, neighbor_long = neighbor_coordinates
            distance = calculate_distance(house_lat, house_long, neighbor_lat, neighbor_long)
            if distance <= MAX_DISTANCE:
                neighboring_houses.append(neighbor_id)
    return neighboring_houses

def process_data(data):
    global house_coordinates, ready_signal_received, suspicion
    try:
        json_data = json.loads(data)
        if 'ID' in json_data and 'Lat' in json_data and 'Long' in json_data:
            house_id = json_data['ID']
            lat = json_data['Lat']
            long = json_data['Long']
            with lock:
                house_coordinates[house_id] = (lat, long)
        elif 'ID' in json_data and 'ConsumoEnergia' in json_data:
            house_id = json_data['ID']
            consumption = json_data['ConsumoEnergia']
            with lock:
                consumption_data[house_id] = consumption
                if consumption == 0:
                    print(f"House {house_id} is SUS")
                if consumption < LOW_CONSUMPTION_THRESHOLD:
                    suspicion[house_id] = suspicion.get(house_id, 0) + SUSPICION_INCREASE_AMOUNT_LOW
                    neighbors = find_neighboring_houses(house_id, house_coordinates)
                    x = 0
                    for neighbor_id in neighbors:
                        neighbor_consumption = consumption_data.get(neighbor_id)
                        if neighbor_consumption is not None and neighbor_consumption > NORMAL_CONSUMPTION_MAX:
                            x += 1
                            print(f"House {house_id} has a neighbour {neighbor_id} with {neighbor_consumption}. SUS")
                            print(f"House {house_id} has {len(neighbors)} neighbours total [{neighbors}]")
                            suspicion[house_id] += SUSPICION_INCREASE_AMOUNT_NEIGHBOUR
                        if len(neighbors) == x:
                            print(f"House {house_id} has all neighbours [{neighbors}] with high consumption. HIGH SUS")
                            suspicion[house_id] += 5*SUSPICION_INCREASE_AMOUNT_NEIGHBOUR
                if house_id in suspicion:
                    s = suspicion.get(house_id)
                    if s >= 50:
                        print(f"Suspicious house ID: {house_id} (Suspicion level: {s})")
                        send_alert_broadcast(house_id, s)
                        del suspicion[house_id]

    except json.JSONDecodeError as e:
        print("Error decoding JSON:", e)

def send_alert_broadcast(house_id, suspicion_level):
    # Broadcast IP address (replace with your desired broadcast IP)
    BROADCAST_IP = "255.255.255.255"

    # Alert message as JSON
    alert_data = {
        "house_id": house_id,
        "suspicion_level": suspicion_level
    }
    alert_json = json.dumps(alert_data)

    try:
        # Create a UDP socket
        sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        sock.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)

        # Send the alert to the broadcast IP on port 3333
        sock.sendto(alert_json.encode("utf-8"), (BROADCAST_IP, 3333))
        print(f"Sent UDP broadcast alert for house {house_id} (Suspicion: {suspicion_level})")
    except (OSError, socket.error) as e:
        print(f"Error sending UDP broadcast alert: {e}")
    finally:
        # Close the socket regardless of exception
        if sock:
            sock.close()

def receive_data(sock, data_queue):
    print("Waiting for data...")
    while True:
        data, addr = sock.recvfrom(1024)
        data_str = data.decode("ascii")
        data_queue.put(data_str)  # Add data to the queue

def data_processor(data_queue):
    while True:
        data_str = data_queue.get()  # Get data from the queue
        process_data(data_str)
        data_queue.task_done()

if __name__ == "__main__":
    UDP_IP = "0.0.0.0"  # IP address on which the mockup is sending UDP packets
    UDP_PORT = 10728      # Port number used by the mockup
    NUM_THREADS = 4     # Number of threads for parallel processing

    data_queue = queue.Queue()  # Create a buffer (queue)

    # Create a UDP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.bind((UDP_IP, UDP_PORT))

    # Create threads
    receive_thread = threading.Thread(target=receive_data, args=(sock, data_queue))
    receive_thread.start()
    for i in range(NUM_THREADS):
        process_thread = threading.Thread(target=data_processor, args=(data_queue,))
        process_thread.start()
    
    