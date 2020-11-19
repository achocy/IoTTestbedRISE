import paho.mqtt.client as mqtt
import pyodbc

cnxn = pyodbc.connect("Driver={ODBC Driver 17 for SQL Server};"
                      "Server=localhost;"  # DESKTOP-CPEF5HP
                      "Database=IoTTestbed;"
                      "Trusted_Connection=yes;")
cursor = cnxn.cursor()


def current_experiment_running():
    cursor.execute("SELECT ExperimentId FROM Experiment WHERE Status='running';")
    row = cursor.fetchone()
    if row is not None:
        print('Current experiment running: ' + str(row.ExperimentId))
    else:
        print('Current experiment running: ' + str(row))

    return None if row is None else row.ExperimentId


def on_connect(client, userdata, flags, rc):
    print("Connected with result code " + str(rc))
    # Subscribing in on_connect() means that if we lose the connection and
    # reconnect then subscriptions will be renewed.
    #client.publish(topic='experiments/next', payload='next', qos=0, retain=False)

    client.subscribe("sensors/+/status")  # PIs are publishing in this topic when an experiment is due

    client.subscribe("experiments/next")  # Web app is publishing in this topic when a user submit new experiment
    client.publish(topic='experiments/next', payload='', qos=0, retain=False)


def on_message(client, userdata, msg):
    if str(msg.topic) == 'experiments/next':
        cur_exp = current_experiment_running()

        if cur_exp is None:
            print('No running experiments')
            cursor.execute("SELECT * FROM Experiment WHERE Status='pending' ORDER BY ExperimentId;")
            experiment = cursor.fetchone()
            if experiment is None:
                print('No scheduled experiments')
                return
            else:
                next_exp = experiment.ExperimentId
                next_exp_duration = int(experiment.Duration)
                # print(next_exp_duration)
                print('The next experiment that should start is ', next_exp)
                sensors = []
                # Publish mqtt message to start the next experiment
                cursor.execute("SELECT SensorId FROM SensorExperiment WHERE ExperimentId=?;",
                               next_exp)  # Fetch sensors for the current experiment

                row = cursor.fetchone()
                while row:
                    sensors.append(row.SensorId)
                    row = cursor.fetchone()

                for s in sensors:
                    print s
                    cursor.execute(
                        "SELECT ProjectName, Filename FROM SensorExperiment WHERE ExperimentId=? AND SensorId=?;",
                        next_exp, s)
                    row = cursor.fetchone()
                    cursor.execute("UPDATE Sensor SET Status='active' WHERE SensorId=?;", s)
                    cnxn.commit()
                    client.publish('sensors/' + str(s), '/home/pi/contikiFirmwares/experiments/' + str(
                        next_exp) + '/' + row.ProjectName + '/' + row.Filename + " " + str(next_exp_duration))

                print('Sensors for current experiment ', sensors)

                cursor.execute("UPDATE Experiment SET Status='running' WHERE ExperimentId=?;", next_exp)
                cnxn.commit()

        # Schedule experiment i.e Set Experiment Status to pending
        if cur_exp is not None:
            print('Current experiment runnning = ', cur_exp)
            print(str(cur_exp) + " must be scheduled")
            cursor.execute("UPDATE Experiment SET Status='pending' WHERE ExperimentId=?;", int(msg.payload))
            cnxn.commit()

    elif str(msg.topic).split('/')[0] == 'sensors':
        print('wtfs')
        cur_exp = current_experiment_running()  # continue here
        print(msg.topic + " " + str(msg.payload))
        sensorId = str(msg.topic).split('/')[1]
        cursor.execute("UPDATE Sensor SET Status='available' WHERE SensorId=?;", sensorId)
        cnxn.commit()

        # this must be changed if we want to execute simultaneous experiments in different sensors.
        # In this i check whether all sensors are available meaning that experiment is finished.
        cursor.execute("SELECT * FROM Sensor Where Status='active';")
        # print(cursor.fetchone())
        if cursor.fetchone() is None:  # No running sensors, experiment is finished
            print('Experiment is finished')
            cursor.execute("UPDATE Experiment SET Status='finished' WHERE ExperimentId=?;",
                           cur_exp)  # change this to Status=finished
            cnxn.commit()
            cursor.execute("UPDATE Experiment SET Log=? WHERE ExperimentId=?;",
                           'C:/Users/Andreas/source/repos/IoTTestbed/IoTTestbed/Logs/aggregator_log_' + str(
                               cur_exp) + '.log', cur_exp)

            cnxn.commit()
            # Start the scheduled experiments according to queue (oldest id and pending)
            client.publish(topic='experiments/next', payload='next', qos=0, retain=False)


client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect("10.16.4.94", 1883, 60)

# Blocking call that processes network traffic, dispatches callbacks and
# handles reconnecting.
# Other loop*() functions are available that give a threaded interface and a
# manual interface.
client.loop_forever()
