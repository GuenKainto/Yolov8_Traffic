CREATE DATABASE Yolov8_Traffic

USE Yolov8_Traffic

CREATE TABLE VehicleCounts(
	dateAndTime DATETIME PRIMARY KEY,
	truck INT,
	bus INT,
	car INT,
	motobike INT,
	bike INT
);

SELECT * FROM VehicleCounts WHERE CONVERT(DATE, dateAndTime) = '2023-09-05';

DELETE FROM VehicleCounts

INSERT INTO VehicleCounts VALUES ()