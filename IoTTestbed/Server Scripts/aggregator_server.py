from __future__ import print_function
import asyncore, socket
import time

import pyodbc

cnxn = pyodbc.connect("Driver={ODBC Driver 17 for SQL Server};"
                      "Server=localhost;"  # DESKTOP-CPEF5HP
                      "Database=IoTTestbed;"
                      "Trusted_Connection=yes;")
cursor = cnxn.cursor()
expId = None


def current_experiment_running():
    cursor.execute("SELECT ExperimentId FROM Experiment WHERE Status='running';")
    row = cursor.fetchone()
    return row if row is None else row.ExperimentId


class Server(asyncore.dispatcher):
    def __init__(self, host, port):
        asyncore.dispatcher.__init__(self)
        self.create_socket(socket.AF_INET, socket.SOCK_STREAM)
        self.bind(('10.16.4.94', port))
        self.listen(5)

    def handle_accept(self):
        # when we get a client connection start a dispatcher for that
        # client
        global expId
        socket, address = self.accept()
        expId = current_experiment_running()
        EchoHandler(socket)
class EchoHandler(asyncore.dispatcher_with_send):
    # dispatcher_with_send extends the basic dispatcher to have an output
    # buffer that it writes whenever there's content
    def handle_read(self):
        self.out_buffer = self.recv(8192)
        lines = self.out_buffer.split('\n')
        lines = [x for x in lines if len(x.strip()) > 0]
        print(len(lines))
        with open('C:/Users/Andreas/source/repos/IoTTestbed/IoTTestbed/Logs/aggregator_log_' + str(expId) + '.log',
                  'a+') as f:
            ts = time.time()
            for l in lines:
                f.write(str(ts) + ';' + str(l) + '\n')
        f.close()

s = Server('10.16.4.94', 5007)

asyncore.loop()
