/*CREATE TABLE service (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    preis INT
);

CREATE TABLE orders (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CustomerName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(50) NOT NULL,
    Priority VARCHAR(50) NOT NULL,
    Status VARCHAR(50) DEFAULT 'Offen',
    ServiceId INT,
    IsDeleted BOOLEAN DEFAULT FALSE,
    
    -- Fremdschlüssel-Constraint auf die `service`-Tabelle
    CONSTRAINT fk_service
    FOREIGN KEY (ServiceId) REFERENCES service(Id)
);*/

/*INSERT INTO service (Id, Name)
VALUES 
    ('1', 'Ski Kleiner Service'),
    ('2','Ski Grosser Service'),
    ('3','Ski Premium Service'),
    ('4','Kleiner Service Kids'),
    ('5','Grosser Service Kids'),
    ('6','Snowboard Kleiner Service'),
    ('7','Snowboard Grosser Service'),
    ('8','Snowboard Premium Service'),
    ('9','Langlauf Kleiner Service'),
    ('10','Langlauf Grosser Service'),
    ('11','Bindung montieren und einstellen'),
    ('12','Fell zuschneiden'),
    ('13','Heisswachsen');
*/

/*INSERT INTO orders (CustomerName, Email, Phone, Priority, ServiceId)
VALUES
    ('Max Mustermann', 'max.mustermann@example.com', '1234567890', 'Standart', 1),
    ('Anna Schmidt', 'anna.schmidt@example.com', '0987654321', 'Express', 3);
*/
/*
SELECT * FROM service;
SELECT * FROM orders;
SELECT * FROM users;
SELECT * FROM userroles;
*/
/*
DROP TABLE orders;
DROP TABLE service;
DROP TABLE users;
DROP TABLE userroles;
*/
/*
SELECT o.Id, o.CustomerName, o.Email, o.Phone, o.Priority, o.Status, s.Name AS service_Name, s.Preis AS Preis
FROM orders o
JOIN service s ON o.ServiceId = s.Id;


ALTER TABLE `orders`
/*ADD COLUMN `create_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;
ADD COLUMN `pickup_date` DATETIME;


UPDATE Service
SET Preis = CASE Id
    WHEN 1 THEN 39.00
    WHEN 2 THEN 59.00
    WHEN 3 THEN 79.00
    WHEN 4 THEN 29.00
    WHEN 5 THEN 39.00
    WHEN 6 THEN 49.00
    WHEN 7 THEN 69.00
    WHEN 8 THEN 89.00
    WHEN 9 THEN 39.00
    WHEN 10 THEN 59.00
    WHEN 11 THEN 25.00
    WHEN 12 THEN 20.00
    WHEN 13 THEN 30.00
    ELSE Preis -- Falls Id nicht übereinstimmt, bleibt der Preis unverändert
END;*/

/*
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL
);

CREATE TABLE UserRoles (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Role VARCHAR(50),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE ON UPDATE CASCADE
);
*/
/*

INSERT INTO users (Username, PasswordHash) VALUES
('admin','password'),
('employee1', 'password1'),
('employee2', 'password2'),
('employee3', 'password3'),
('employee4', 'password4'),
('employee5', 'password5'),
('employee6', 'password6'),
('employee7', 'password7'),
('employee8', 'password8'),
('employee9', 'password9'),
('employee10', 'password10');
INSERT INTO UserRoles (UserId, Role) VALUES 
('1', 'Admin'),
('2', 'Employee'),
('3', 'Employee'),
('4', 'Employee'),
('5', 'Employee'),
('6', 'Employee'),
('7', 'Employee'),
('8', 'Employee'),
('9', 'Employee'),
('10', 'Employee'),
('11', 'Employee');


CREATE TABLE Logs (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    Endpoint VARCHAR(255) NOT NULL,
    HttpMethod VARCHAR(10) NOT NULL,
    StatusCode INT NOT NULL,
    Message TEXT
);
*/



