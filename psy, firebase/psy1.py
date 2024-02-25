import matplotlib.pyplot as plt
import firebase_admin
from firebase_admin import credentials, db
import warnings
warnings.filterwarnings("ignore")
import re
import pandas as pd
import numpy as np
from sklearn.linear_model import LinearRegression
import pandas as pd
import numpy as np
from sklearn.linear_model import LinearRegression
from sklearn.model_selection import train_test_split
from sklearn import svm
import matplotlib.pyplot as plt


# Initialize Firebase
cred = credentials.Certificate(r"./projectpsy-815fa-firebase-adminsdk-6jl97-39e99230ab.json")
firebase_admin.initialize_app(cred, {
    'databaseURL': 'https://projectpsy-815fa-default-rtdb.asia-southeast1.firebasedatabase.app/'
})

 #Create empty lists to store data points
timestamps = []
max_values = []
min_values = []

# Define patterns for timestamp and min/max values
timestamp_pattern = r"(\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM))"
min_max_pattern = r"max:(\d+), min:(\d+)"

# Callback function to collect data points
def listener(event):
    data = event.data

    if data is None:
        print("No data received")
        return

    for name, inner_dict in data.items():
        for id, value in inner_dict.items():
            # Check if value is a string
            if isinstance(value, str):
                timestamp_match = re.search(timestamp_pattern, name)
                min_max_match = re.search(min_max_pattern, value)

                timestamp = timestamp_match.group(1) if timestamp_match else None
                max_value = float(min_max_match.group(1)) if min_max_match else None
                min_value = float(min_max_match.group(2)) if min_max_match else None

                if timestamp and max_value and min_value:
                    timestamps.append(timestamp)
                    max_values.append(max_value)
                    min_values.append(min_value)
                    print(f"Timestamp: {timestamp}, Max: {max_value}, Min: {min_value}")
                    
                   # range=[]
   # for i,j in zip(max_values,min_values):
    #    range.append(i-j)
   # fig, ax = plt.subplots()
   # ax.plot(timestamps,range)
    # plt.show()
    
    range = [a - b for a, b in zip(max_values, min_values)]
    range1=range
    
    range=range[:4]
    data=pd.read_csv(r"./patient.csv")
    data=data.drop('Name',axis=1)
    X=data.drop('Time',axis=1)
    Y=data['Time']
    X_train,X_test,Y_train,Y_test=train_test_split(X,Y,test_size=0.2,random_state=2)
    model=svm.SVC(kernel='linear')
    model.fit(X_train,Y_train)
    input=range
    input_array=np.asarray(input)
    input_reshape=input_array.reshape(1,-1)
    print(input_reshape)
    prediction=model.predict(input_reshape)
    prediction_value=int(prediction)
    print("The predicted number of weeks for the patient to attain maximum range of motion if the exercise is continued is:")
    print(prediction_value)
    fig, ax = plt.subplots()
    ax.plot(timestamps, range1, marker='o', color='b')
    plt.xlabel('Timestamps')
    plt.ylabel('Range')
    plt.title('Range vs Timestamps')
    plt.xticks(rotation=45)  # Rotate x-axis labels for better visibility
    plt.tight_layout()
    text_label = "The predicted number of weeks for the patient to attain maximum range of motion if the exercise is continued is  "
    x_text, y_text = 0.5, 42 # Coordinates for the text label
    x_variable, y_variable = 6, 42  # Coordinates for the variable value
    plt.text(x_text, y_text, text_label, fontsize=12, color='green')
    plt.text(x_variable, y_variable, str(prediction_value), fontsize=12, color='red')

    plt.show()
    
   

    


# Database reference
name=input('Enter the patients name')
ref = db.reference(name)
ref.listen(listener)


#print(max_values)
