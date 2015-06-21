import socket

PCIP = None
MOBIP = None
MESSAGES = "AA Gya"


UDP_IP = ""
UDP_PORT = 12345

sock = socket.socket(socket.AF_INET, 
                     socket.SOCK_DGRAM) 
sock.bind((UDP_IP, UDP_PORT))

while True:
    data, addr = sock.recvfrom(1024)
    recMsgData = data.decode("UTF-8")
    if(recMsgData == "PC"):
    	PCIP = addr[0]
    	PCPORT = addr[1]
    	print("Message from PC")
    elif(recMsgData == "MOB"):
    	MOBIP = addr[0]
    	MOBPORT = addr[1]
    	print("Message from MOB")
    else:
    	print("Ignoring Non-Windroid Request...")
    if(PCIP and MOBIP is not None):
    	PCCONFIG = str(PCIP) + ":" + str(PCPORT)
    	MOBCONFIG = str(MOBIP) + ":" + str(MOBPORT)
    	sock.sendto(bytes(MOBCONFIG, "utf-8"), (PCIP, PCPORT))
    	sock.sendto(bytes(PCCONFIG, "utf-8"), (MOBIP, MOBPORT))
    	PCIP = None
    	MOBIP = None
    	PCPORT = None
    	MOBPORT = None
    	print("HOLE PUNCHING DONE")

   # print(recMsgData)
   # print ("received message:", data)
   # print ("Connection Details: ", addr)
