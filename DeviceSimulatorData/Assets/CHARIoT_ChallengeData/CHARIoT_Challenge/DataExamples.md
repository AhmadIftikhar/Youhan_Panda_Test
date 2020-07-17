# Overview
These are sample data return from sensor devices used during the CHARIoT Challenge event.  Some sensors and feeds are 
used across multiple scenarios and are listed as such.  These may include 'future tech' sensors that are based on real 
world sensors where possible and may include features or data not seen in current generation to provide more robust data
for teams during the challenge.

In some cases, output from an analytics engine is noted. This is assumed to be the output after processing raw data from 
some upstream sensor, such as a camera on a drone.  

In some scenarios the actual source device may be the same for different analytic returns.  (i.e. different analytics 
are processed from the same camera feeds or drone imagery)

## Sensors Used in Every Scenario
### TrafficConditions

Average road speed sensor assumed to detect average speed of vehicles along a roadway provided as a linestring
in geometry.  Assumed to be sensors embedded along roadway.

Common message rate: 1 per minute

**Message Fields**
```
"deviceid": string of device id
"type": string of note on device type
"time": string of UTC datetime
"street": optional string of roadway name
"average": int of speed in mph,
"geometry": dict of geojson data
    "type": string of geometry type
    "coordinates": list of longitude, latitude point tracing a roadway segment
```


**Example Return**
```json
{
    "deviceid": "692fb0411125460f921b9fdd372d83db",
    "type": "Road segment average traffic speed in mph",
    "time": "2020-06-05 16:14:23.185095Z",
    "street": "East Cary Street",
    "average": 35,
    "geometry": {
        "type": "LineString",
        "coordinates": [
            [
                -77.4311691,
                37.5337325
            ],
            [
                -77.4306741,
                37.5334239
            ],
            [
                -77.4302241,
                37.5331435
            ],
            [
                -77.429333,
                37.5325898
            ]
        ]
    }
}
```

### RoadClosure

Assumed to be IoT array involved in setting indicators that a roadway segment is closed to traffic.  Assumed to be
and IoT controller that regulates signs, gates, and other devices that would indicate a roadway closure.  Segement
provided via linestring in geometry.

Common message rate: 1 per minute.

**Message Fields**
```
"deviceid": string of device id
"type": string of sensor type description
"alert_type": string of alert type.  One of "ROAD_CLOSED"|"ROAD_OPEN"
"start_time": string of UTC datetime of start of closure
"end_time": string of UTC datetime of end of closure
"street": Optional string of street name
"geometry": dict of geojson data
    "type": string of geometry type
    "coordinates": list of longitude, latitude point tracing a roadway segment
```


**Example Return**
```json
{
    "deviceid": "a62e0747fc874e4089e2149602fc39e1",
    "type": "Road segment closure alert",
    "alert_type": "ROAD_CLOSED",
    "start_time": "2020-06-05 16:14:23.185095Z",
    "end_time": "2020-06-05 19:14:23.185095Z",
    "street": "East Cary Street",
    "geometry": {
        "type": "LineString",
        "coordinates": [
            [
                -77.4311691,
                37.5337325
            ],
            [
                -77.4306741,
                37.5334239
            ],
            [
                -77.4302241,
                37.5331435
            ],
            [
                -77.429333,
                37.5325898
            ]
        ]
    }
}
```

## Flood Sensors Data Returns
### RoadwayWaterLevel

Roadway flood sensor device, assumed to be a device that sits above the roadway and uses ultrasonic signals to
detect height of water above the road surface.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"depth": {
    "value": float value of water depth above roadway
    "units": string of unit
```


**Example Return**
```json
{
    "type": "Roadway water level sensor",
    "deviceid": "b64e85067d7a42dba06014924574c0af",
    "time": "2020-06-05 16:14:23.185095Z",
    "coords": {
        "latitude": 37.53236,
        "longitude": -77.4306
    },
    "depth": {
        "value": 0.54,
        "units": "meters"
    }
}
```

### RiverWaterLevel

River water level sensor.  Assumes a mounted device above the river that uses ultrasonic or LIDAR to measure
water level above flood stage and water speed.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"level":
    "value": float of measurement above flood stage, 0 if not above or below flood stage.
    "units": string of units
"speed": {
    "value": float of measured surface waterspeed
    "units": string of units used
```


**Example Return**
```json
{
    "deviceid": "4c5741f9637e49cfada745454e52b3c5",
    "type": "river water level sensor",
    "level": {
        "value": 0.85,
        "units": "meters above flood"
    },
    "speed": {
        "value": 3.23,
        "units": "meters per second"
    },
    "time": "2020-06-05 16:14:23.185095Z",
    "coords": {
        "latitude": 37.53041,
        "longitude": -77.43111
    }
}
```

### RainGauge

Rain gauge sensor that measures accumulated rainfall at a point over a period of time.

Common message rate: 1 per minute

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"period":
    "start": string of UTC Time measuring period began
    "end": string of UTC Time measuring period ended
"time": string of UTC time data was reported
"accumulation":
    "value": float of accumulated rain during period
    "units": string of units
```


**Example Return**
```json
{
    "deviceid": "3eed3121386c4162b8288752a9314653",
    "type": "Rain Guage",
    "period": {
        "start": "2020-06-05 16:14:23.185095Z",
        "end": "2020-06-05 16:14:23.185095Z"
    },
    "time": "2020-06-05 16:14:23.185095Z",
    "accumulation": {
        "value": 3.49,
        "units": "cm"
    },
    "coords": {
        "latitude": 37.52927,
        "longitude": -77.43236
    }
}
```

### WaterQuality

Water quality sensor.  Assumed to be a deployed device with probe that measures water conditions.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"ph_level":
    "value": float of ph value
    "units": string of units
"conductivity": {
    "value": float of value
    "units": string of units
"orp_level": {
    "value": float of value
    "units": string of units
```


**Example Return**
```json
{
    "deviceid": "66186396ddce42daba99d59d7447dd75",
    "type": "Water quality sensor",
    "ph_level": {
        "value": 8.45,
        "units": "pH"
    },
    "conductivity": {
        "value": 884.34,
        "units": "mS/cm"
    },
    "orp_level": {
        "value": 894.74,
        "units": "mV"
    },
    "time": "2020-06-05 16:14:23.185095Z",
    "coords": {
        "latitude": 37.53128,
        "longitude": -77.42884
    }
}
```

### PeopleSpotterAI

Simulated output from Image analytics AI that detects presence of people.  This is assumed to be an output from an
analytics engine processing video from mounted cameras or drones to identify the presence of people in the frame.
It returns the number of people in a location but does not identify or track people across sensors or frames.

Common message rate: as needed, when a positive event occurs

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"count": int of number of people detected
```


**Example Return**
```json
{
    "deviceid": "8a7ff210d02d4b5890cefc0ac9ee2a24",
    "type": "Analytics people counter",
    "count": 2,
    "time": "2020-06-05 16:14:23.185095Z",
    "coords": {
        "latitude": 37.53238,
        "longitude": -77.43049
    }
}
```

### VitalMonitoringSensors

Wearable Vital Monitoring sensor attached to measure vital signs.  SOS is assumed to be triggered if spo2 or
pulse fall outside configured boundaries.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords": {
    "latitude": float of latitude
    "longitude": float of longitude
"sos": boolean of sos status
"spo2":
    "value": float of pulse oxygen value
    "unit": string of units
"pulse":
    "value": int of pulse value
    "unit": string of units
```


**Example Return**
```json
{
    "deviceid": "470e9f8ddd7b42c686eac95881bd88a8",
    "type": "Personal Pulse Oxgination Sensor",
    "time": "2020-06-05 16:14:23.185095Z",
    "spo2": {
        "value": 98,
        "unit": "%"
    },
    "pulse": {
        "value": 64,
        "unit": "beats per min"
    },
    "coords": {
        "latitude": 37.53284,
        "longitude": -77.43036
    },
    "sos": true
}
```

### SonarSensor

Simulation of an analytics feed from a submersible sensor that returns a record if and underwater object is detected
along it's path.  Deployed to identify cars or other objects of interest beneith the surface for first responders
to clear.

Common message rate:  as needed, when a postive event occurs.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"object_detected": boolean of detection
"depth":
    "value": float of depth below surface
    "units": string of units
```


**Example Return**
```json
{
    "type": "Submersible sonar detector",
    "deviceid": "c9334d156a9245ff897128bd4fc5c3dd",
    "object_detected": true,
    "depth": {
        "value": 1.35,
        "units": "meters"
    },
    "time": "2020-06-05 16:14:23.185095Z",
    "coords": {
        "latitude": 37.53228,
        "longitude": -77.43078
    }
}
```

## Active Shooter Sensors Data Returns
### ShotSpotter

Simulation of a ShotSpotter service return.  This is assumed to be a central serivce that generates an alert based
on the analysis of an IoT array in the environment to detect and identify possible sounds of gunshots and provides
a triangulated location for the event.

Common message rate: as needed, when a positive event occurs.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"event_type": string of event type.  One of 'Gunshots_or_Firecracker'|'Single_Gunshot'|'Multiple_Gunshots'
```


**Example Return**
```json
{
    "deviceid": "cd62bbd7953946edb16cbbe5fd99a83d",
    "type": "Shot spottor sensor",
    "coords": {
        "latitude": 37.53236,
        "longitude": -77.42754
    },
    "time": "2020-06-05 16:14:23.188096Z",
    "event_type": "Gunshots_or_Firecracker"
}
```

### OccupancySensor

Simulation of a device or visual computing based algorithm that counts the number of occupants in a space.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"count": int of number of people
```


**Example Return**
```json
{
    "deviceid": "c3bfc7bb8760430daec132e7f9b76a3d",
    "type": "occupancy counter",
    "time": "2020-06-05 16:14:23.188096Z",
    "count": 3,
    "coords": {
        "latitude": 37.53244,
        "longitude": -77.42772
    }
}
```

### SuspiciousObjectDetectionAI

Simulation of an AI that uses computer vision or human mediated system that marks objects to alert for suspicious
packages or potential IEDs in an area.

Common message rate: as needed, when a positive event occurs.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"alert_type": string of alert type.  One of 'Suspected Explosive Device'|'Object of Interest'
```


**Example Return**
```json
{
    "deviceid": "cc427dd205b6418aabf647633324d581",
    "type": "Suspicious object detection",
    "time": "2020-06-05 16:14:23.188096Z",
    "alert_type": "Suspected Explosive Device",
    "coords": {
        "latitude": 37.53244,
        "longitude": -77.4277
    }
}
```

### DoorwayStatus

Simple sensor that indicates doorway status as open/close and locked/unlocked.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"locked": boolean of lock status
"shut": boolean of shut status
```


**Example Return**
```json
{
    "deviceid": "a49f9730b7fa46358e4d2f83ee299ecd",
    "type": "Doorway Status Sensor",
    "time": "2020-06-05 16:14:23.188096Z",
    "locked": false,
    "shut": true,
    "coords": {
        "latitude": 37.53242,
        "longitude": -77.42767
    }
}
```

### OfficerVitalMonitoring

Wearable Vital Monitoring Sensor for police officers and first responders that also provides data on weapon status
and shots fired.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"sos": boolean of sos status
"spo2":
    "value": float of pulse oxygen value
    "unit": string of units
"pulse":
    "value": int of pulse value
    "unit": string of units
"weapon_drawn": boolean of status of weapon drawn from holster
"shots_fired": int of number of shots fired
```


**Example Return**
```json
{
    "deviceid": "28c537e8606b4f95922d97834babd1c3",
    "type": "Personal Pulse Oxgination Sensor",
    "time": "2020-06-05 16:14:23.188096Z",
    "spo2": {
        "value": 98,
        "unit": "%"
    },
    "pulse": {
        "value": 125,
        "unit": "beats per min"
    },
    "coords": {
        "latitude": 37.53242,
        "longitude": -77.42769
    },
    "sos": false,
    "weapon_drawn": true,
    "shots_fired": 3
}
```

## Wildfire Sensor Data Returns
### AerialThermalDetector

Simulation of an output from computer vision based analytics attached to a drone using arial imagery to detect
thermal hotspots indicating fires.

Common message rate: 1 per second

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"thermal_threshold":
    "value": float of temprature reading
    "units": string of units
"radius":
    "value": float of radius of thermal reading from centerpoint coords
    "units": string of units
```


**Example Return**
```json
{
    "type": "Realtime thermal imagery analytics from aerial drone",
    "deviceid": "8210209740cb4476b7e2b80441ad7ebf",
    "time": "2020-06-05 16:14:23.189098Z",
    "coords": {
        "latitude": 37.52239,
        "longitude": -77.47054
    },
    "thermal_threshold": {
        "value": 800.6,
        "units": "celcius"
    },
    "radius": {
        "value": 91.5,
        "units": "meters"
    }
}
```

### PeopleSpotterAI

Simulated output from Image analytics AI that detects presence of people.  This is assumed to be an output from an
analytics engine processing video from mounted cameras or drones to identify the presence of people in the frame.
It returns the number of people in a location but does not identify or track people across sensors or frames.

Common message rate: as needed, when a positive event occurs

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"count": int of number of people detected
```


**Example Return**
```json
{
    "deviceid": "8210209740cb4476b7e2b80441ad7ebf",
    "type": "Analytics people counter",
    "count": 1,
    "time": "2020-06-05 16:14:23.189098Z",
    "coords": {
        "latitude": 37.52202,
        "longitude": -77.46963
    }
}
```

### ObjectIdentifier

Simulation of an output from computer vision analytics attached to a drone to detect structures and cars in a
selected region that may need to be cleared by first responders.

Common message rate: as needed, when a positive event occurs.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"object_type": string of detected object. One of 'POSSIBLE BUILDING'|'POSSIBLE CAR'
```


**Example Return**
```json
{
    "type": "Realtime analytics to identify structures and cars",
    "deviceid": "8210209740cb4476b7e2b80441ad7ebf",
    "time": "2020-06-05 16:14:23.189098Z",
    "coords": {
        "latitude": 37.52193,
        "longitude": -77.46992
    },
    "object_type": "POSSIBLE CAR"
}
```

### Anemometer

Windspeed and direction sensor.

Common message rate: 1 per second

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"wind_direction":
    "value": float of compass angle of wind direction
    "units": string of units
"wind_speed":
    "value": float of windspeed
    "units": string of units
```


**Example Return**
```json
{
    "type": "Anemometer",
    "deviceid": "21378eda155d494a8718b355a9499250",
    "time": "2020-06-05 16:14:23.189098Z",
    "coords": {
        "latitude": 37.52405,
        "longitude": -77.46865
    },
    "wind_speed": {
        "value": 3.9,
        "units": "m/s"
    },
    "wind_direction": {
        "value": 194.4,
        "units": "degrees"
    }
}
```

### AirQualityGasSensor

Air Quality sensor for dangerous air during fire.  Assumed to be a connected multi-gas device.  May be deployed
or mounted alongside other devices such as an anemometer.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"o2":
    "value": float of percent 02 detected
    "units": string of units
    "min_threshold": float of value to generate alert at or below
    "max_threshold": float of value to generate alert at or above
},
"co": {
    "value": float of reading
    "units": "ppm",
    "max_threshold": float of value to generate alert at or above
},
"h2s": {
    "value": float of reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
},
"hcn": {
    "value": float of reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
},
"lel": {
    "value": float of reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
},
"particulant": {
    "value": float or reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
```


**Example Return**
```json
{
    "deviceid": "a63a18c95c7f46b8a95ae0f020ee91e8",
    "type": "Air Quality and Flamable Gas Sensor",
    "time": "2020-06-05 16:14:23.189098Z",
    "o2": {
        "value": 18.5,
        "units": "%",
        "min_threshold": 19.0,
        "max_threshold": 30.0
    },
    "co": {
        "value": 36.4,
        "units": "ppm",
        "max_threshold": 10.0
    },
    "h2s": {
        "value": 1.0,
        "units": "ppm",
        "max_threshold": 5.0
    },
    "hcn": {
        "value": 5.3,
        "units": "ppm",
        "max_threshold": 4.7
    },
    "lel": {
        "value": 82.2,
        "units": "%",
        "max_threshold": 80.0
    },
    "particulate": {
        "value": 230,
        "units": "mg/m3",
        "max_threshold": 150.0
    },
    "coords": {
        "latitude": 37.52062,
        "longitude": -77.46906
    }
}
```

### FirstResponderVitalMonitor

Wearable Vital Monitoring Sensor for first responders.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"sos": boolean of sos status
"spo2":
    "value": float of pulse oxygen value
    "unit": string of units
"pulse":
    "value": int of pulse value
    "unit": string of units
```


**Example Return**
```json
{
    "deviceid": "268de0b9d07a4b6cab1aa0bc17fb0eba",
    "type": "Firefighter or EMS vitals monitoring sensor",
    "time": "2020-06-05 16:14:23.189098Z",
    "spo2": {
        "value": 95.3,
        "unit": "%"
    },
    "pulse": {
        "value": 76,
        "unit": "beats per min"
    },
    "coords": {
        "latitude": 37.52062,
        "longitude": -77.46905
    },
    "sos": false
}
```

## Mass Transit Accident Sensor Data Returns
### VoltageSensor

Mounted device along a railway or powerline that indicates the level of voltage in that segment.

Common message rate: 4 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"voltage": float of volts measured
```


**Example Return**
```json
{
    "deviceid": "a6f70524d33b4e52ac0d1918397375b8",
    "type": "Industrial voltage detector",
    "time": "2020-06-05 16:14:23.190096Z",
    "coords": {
        "latitude": 37.53166,
        "longitude": -77.43143
    },
    "voltage": 600
}
```

### RadiationDetector

Werable alarm that alerts for the presence of potentially harmful levels of radiation.

Common message rate: 2 per second.

**Message Fields**
```
"deviceid": string of id for device
"time": string of UTC Time
"type": "Personal Dosimeter",
"alarm": boolean of alarm state,
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
```


**Example Return**
```json
{
    "deviceid": "a5f31871d9184148b90950467d3579c1",
    "time": "2020-06-05 16:14:23.190096Z",
    "type": "Personal Dosimeter",
    "alarm": false,
    "coords": {
        "latitude": 37.53187,
        "longitude": -77.43126
    }
}
```

### AirQualityGasSensor

Air Quality sensor for dangerous air during fire.  Assumed to be a connected multi-gas device.  May be deployed
or mounted alongside other devices such as an anemometer.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"o2":
    "value": float of percent 02 detected
    "units": string of units
    "min_threshold": float of value to generate alert at or below
    "max_threshold": float of value to generate alert at or above
},
"co": {
    "value": float of reading
    "units": "ppm",
    "max_threshold": float of value to generate alert at or above
},
"h2s": {
    "value": float of reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
},
"hcn": {
    "value": float of reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
},
"lel": {
    "value": float of reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
},
"particulant": {
    "value": float or reading
    "units": string of units
    "max_threshold": float of value to generate alert at or above
```


**Example Return**
```json
{
    "deviceid": "04afbeefec9149c99d73c6546ca1cbc3",
    "type": "Air Quality and Flamable Gas Sensor",
    "time": "2020-06-05 16:14:23.191097Z",
    "o2": {
        "value": 18.5,
        "units": "%",
        "min_threshold": 19.0,
        "max_threshold": 30.0
    },
    "co": {
        "value": 36.4,
        "units": "ppm",
        "max_threshold": 10.0
    },
    "h2s": {
        "value": 1.0,
        "units": "ppm",
        "max_threshold": 5.0
    },
    "hcn": {
        "value": 5.3,
        "units": "ppm",
        "max_threshold": 4.7
    },
    "lel": {
        "value": 82.2,
        "units": "%",
        "max_threshold": 80.0
    },
    "particulate": {
        "value": 230,
        "units": "mg/m3",
        "max_threshold": 150.0
    },
    "coords": {
        "latitude": 37.53186,
        "longitude": -77.43133
    }
}
```

### CasualtyDetectorAI

Simulated output from computer vision algorithm that identifies possible casualties on the ground through
processing video from drones or mounted cameras, or from a human mediated system that marks potential victims.

Common message rate: as needed, when a postive event occurs.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
```


**Example Return**
```json
{
    "deviceid": "223bf75e802e4dac953b0c78b487df75",
    "type": "Drone based visual AI to detect possible victims",
    "time": "2020-06-05 16:14:23.191097Z",
    "coords": {
        "latitude": 37.53166,
        "longitude": -77.43135
    }
}
```

### VitalMonitoringSensors

Wearable Vital Monitoring sensor attached to measure vital signs.  SOS is assumed to be triggered if spo2 or
pulse fall outside configured boundaries.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords": {
    "latitude": float of latitude
    "longitude": float of longitude
"sos": boolean of sos status
"spo2":
    "value": float of pulse oxygen value
    "unit": string of units
"pulse":
    "value": int of pulse value
    "unit": string of units
```


**Example Return**
```json
{
    "deviceid": "eb52c0bba6e44c75b396a55733c8a13c",
    "type": "Personal Pulse Oxgination Sensor",
    "time": "2020-06-05 16:14:23.191097Z",
    "spo2": {
        "value": 93.5,
        "unit": "%"
    },
    "pulse": {
        "value": 76,
        "unit": "beats per min"
    },
    "coords": {
        "latitude": 37.53173,
        "longitude": -77.43114
    },
    "sos": false
}
```

### OfficerVitalMonitoring

Wearable Vital Monitoring Sensor for police officers and first responders that also provides data on weapon status
and shots fired.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"sos": boolean of sos status
"spo2":
    "value": float of pulse oxygen value
    "unit": string of units
"pulse":
    "value": int of pulse value
    "unit": string of units
"weapon_drawn": boolean of status of weapon drawn from holster
"shots_fired": int of number of shots fired
```


**Example Return**
```json
{
    "deviceid": "dc8c033c865b43049b505a2845b94e49",
    "type": "Personal Pulse Oxgination Sensor",
    "time": "2020-06-05 16:14:23.191097Z",
    "spo2": {
        "value": 95.3,
        "unit": "%"
    },
    "pulse": {
        "value": 76,
        "unit": "beats per min"
    },
    "coords": {
        "latitude": 37.5318,
        "longitude": -77.43134
    },
    "sos": false,
    "weapon_drawn": false,
    "shots_fired": 0
}
```

### OccupancySensor

Simulation of a device or visual computing based algorithm that counts the number of occupants in a space.

Common message rate: 1 per second.

**Message Fields**
```
"type": string of sensor type
"deviceid": unique identifier of device
"time": string of UTC Time
"coords":
    "latitude": float of latitude
    "longitude": float of longitude
"count": int of number of people
```


**Example Return**
```json
{
    "deviceid": "c9e9a50aca834eff8c8c91015d70089b",
    "type": "occupancy counter",
    "time": "2020-06-05 16:14:23.191097Z",
    "count": 8,
    "coords": {
        "latitude": 37.53175,
        "longitude": -77.43143
    }
}
```
