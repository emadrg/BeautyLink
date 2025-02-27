
CREATE DATABASE Salon;

USE Salon;

CREATE TABLE [Role] (
Id TINYINT NOT NULL IDENTITY,
[Name] NVARCHAR(250),
CONSTRAINT pk_role PRIMARY KEY (Id)
);

CREATE TABLE [AppFile] ( 
Id INT NOT NULL IDENTITY,
[Path] NVARCHAR(1000) NOT NULL,
Extesion NVARCHAR(10) NOT NULL,
[Name] NVARCHAR(50) NOT NULL,
CONSTRAINT pk_file PRIMARY KEY (Id)
);

CREATE TABLE UserStatus(
Id INT NOT NULL,
[Name] VARCHAR (100) NOT NULL,
CONSTRAINT pk_user_status PRIMARY KEY (Id)
);


CREATE TABLE SalonStatus(
Id INT NOT NULL,
[Name] VARCHAR (100) NOT NULL,
CONSTRAINT pk_salon_status PRIMARY KEY (Id)
);


CREATE TABLE [User] (
Id UNIQUEIDENTIFIER NOT NULL,
FirstName NVARCHAR(50) NOT NULL,
LastName NVARCHAR(50),
Email NVARCHAR(250) NOT NULL,
CreatedBy UNIQUEIDENTIFIER,
CreatedDate DATETIME NOT NULL,
LastModifiedBy UNIQUEIDENTIFIER,
LastModifiedDate DATETIME NOT NULL,
[Password] NVARCHAR(250) NOT NULL,
RoleId TINYINT NOT NULL,
StatusId INT NOT NULL,
ProfilePictureId INT,
PhoneNumber NVARCHAR(15),
CONSTRAINT pk_user PRIMARY KEY (Id),
CONSTRAINT fk_user_created_by FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
CONSTRAINT fk_user_last_modified_by FOREIGN KEY (LastModifiedBy) REFERENCES [User](Id),
CONSTRAINT fk_user_role FOREIGN KEY (RoleId) REFERENCES [Role](Id),
CONSTRAINT fk_user_file FOREIGN KEY (ProfilePictureId) REFERENCES AppFile(Id),
CONSTRAINT fk_user_status FOREIGN KEY (StatusId) REFERENCES UserStatus(Id),
CONSTRAINT fk_user_profile_picture FOREIGN KEY (ProfilePictureId) REFERENCES AppFile(Id)
);

CREATE TABLE [Log] (
Id INT NOT NULL IDENTITY,
LogLevel TINYINT NOT NULL,
ErrorMessage NVARCHAR(1000) NOT NULL,
StackTrace NVARCHAR(MAX) NOT NULL,
CreatedBy UNIQUEIDENTIFIER,
CreatedDate DATETIME NOT NULL,
CONSTRAINT pk_table PRIMARY KEY (Id),
CONSTRAINT fk_log_user FOREIGN KEY (CreatedBy) REFERENCES [User](Id)
);


CREATE TABLE County ( 
Id INT NOT NULL IDENTITY,
[Name] VARCHAR (100) NOT NULL,
CONSTRAINT pk_county PRIMARY KEY (Id)
);


CREATE TABLE City ( 
Id INT NOT NULL IDENTITY,
ContyId INT NOT NULL,
[Name] VARCHAR (100) NOT NULL,
CONSTRAINT pk_city PRIMARY KEY (Id),
CONSTRAINT fk_city_county FOREIGN KEY (ContyId) REFERENCES County(Id)
);

CREATE TABLE Salon (
Id INT NOT NULL IDENTITY,
[Name] VARCHAR (100) NOT NULL,
CountyId INT NOT NULL,
CityId INT NOT NULL,
[Address] NVARCHAR(100),
StatusId INT NOT NULL,
CONSTRAINT pk_salon PRIMARY KEY (Id),
CONSTRAINT fk_salon_county FOREIGN KEY (CountyId) REFERENCES County(Id),
CONSTRAINT fk_salon_city FOREIGN KEY (CityId) REFERENCES City(Id),
CONSTRAINT fk_salon_status FOREIGN KEY (StatusId) REFERENCES SalonStatus(Id)
);

CREATE TABLE SalonPicture (
SalonId INT NOT NULL, 
FileId INT NOT NULL,
CONSTRAINT pk_salon_picture PRIMARY KEY (SalonId, FileId),
CONSTRAINT fk_salon_picture_salon FOREIGN KEY (SalonId) REFERENCES Salon(Id),
CONSTRAINT fk_salon_picture_file FOREIGN KEY (FileId) REFERENCES AppFile(Id),
);

CREATE TABLE SalonRegistrationDocument (
SalonId INT NOT NULL, 
FileId INT NOT NULL,
CONSTRAINT pk_salon_registration_document PRIMARY KEY (SalonId, FileId),
CONSTRAINT fk_salon_registration_document_salon FOREIGN KEY (SalonId) REFERENCES Salon(Id),
CONSTRAINT fk_salon_registration_document_file FOREIGN KEY (FileId) REFERENCES AppFile(Id),
);

CREATE TABLE Manager (
Id UNIQUEIDENTIFIER NOT NULL,
UserId UNIQUEIDENTIFIER NOT NULL,
SalonId INT NOT NULL,
CONSTRAINT pk_manager PRIMARY KEY (Id),
CONSTRAINT fk_manager_user FOREIGN KEY (UserId) REFERENCES [User](Id),
CONSTRAINT fk_manager_salon FOREIGN KEY (SalonId) REFERENCES Salon(Id)
);

CREATE TABLE Stylist (
Id UNIQUEIDENTIFIER NOT NULL,
UserId UNIQUEIDENTIFIER NOT NULL,
SalonId INT NOT NULL,
SocialMediaLink NVARCHAR(500),
CONSTRAINT pk_stylist PRIMARY KEY (Id),
CONSTRAINT fk_stylist_user FOREIGN KEY (UserId) REFERENCES [User](Id),
CONSTRAINT fk_stylist_salon FOREIGN KEY (SalonId) REFERENCES Salon(Id)
);

CREATE TABLE UnavailableTime (
Id INT NOT NULL IDENTITY,
StartDate DATETIME NOT NULL,
EndDate DATETIME NOT NULL,
Reason NVARCHAR(50),
StylistId UNIQUEIDENTIFIER NOT NULL,
CONSTRAINT pk_unavailableTime PRIMARY KEY (Id),
CONSTRAINT fk_unavailableTime_stylist FOREIGN KEY (StylistId) REFERENCES Stylist(Id)
);


CREATE TABLE [WeekDay] (
Id INT NOT NULL IDENTITY,
[Name] NVARCHAR(15),
CONSTRAINT pk_week_day PRIMARY KEY (Id)
);

CREATE TABLE Schedule (
Id INT NOT NULL IDENTITY,
StylistId UNIQUEIDENTIFIER,
WeekDayId INT NOT NULL,
StartTime TIME NOT NULL,
EndTime TIME NOT NULL,
CONSTRAINT pk_schedule PRIMARY KEY (Id),
CONSTRAINT fk_schedule_stylist FOREIGN KEY (StylistId) REFERENCES Stylist(Id),
CONSTRAINT fk_schedule_week_day FOREIGN KEY (WeekDayId) REFERENCES [WeekDay](Id)
);

CREATE TABLE [Service] (
Id INT NOT NULL IDENTITY,
[Name] NVARCHAR(100) NOT NULL,
CONSTRAINT pk_service PRIMARY KEY (Id)
);

CREATE TABLE ServiceStylist (
Id INT NOT NULL IDENTITY,
ServiceId INT NOT NULL,
StylistId UNIQUEIDENTIFIER NOT NULL,
DurationMinutes INT NOT NULL,
Price DECIMAL NOT NULL,
CONSTRAINT pk_service_stylist PRIMARY KEY (Id),
CONSTRAINT uq_service_stylist UNIQUE (ServiceId, StylistId),
CONSTRAINT fk_service_stylist_service FOREIGN KEY (ServiceId) REFERENCES [Service](Id),
CONSTRAINT fk_service_stylist_stylist FOREIGN KEY (StylistId) REFERENCES Stylist(Id)
);

CREATE TABLE AppointmentStatus (
Id INT NOT NULL IDENTITY,
[Name] NVARCHAR(15) NOT NULL,
CONSTRAINT pk_status PRIMARY KEY (Id),
);

CREATE TABLE Appointment (
Id INT NOT NULL IDENTITY,
ClientId UNIQUEIDENTIFIER NOT NULL,
StatusId INT NOT NULL, 
StartDate DATETIME NOT NULL,
EndDate DATETIME NOT NULL,
CONSTRAINT pk_appointment PRIMARY KEY (Id),
CONSTRAINT fk_appointment_status FOREIGN KEY (StatusId) REFERENCES AppointmentStatus(Id),
CONSTRAINT fk_appointment_service_client FOREIGN KEY (ClientId) REFERENCES [User](Id)
);

CREATE TABLE AppointmentStylistService(
AppointmentId INT NOT NULL,
ServiceStylistId INT NOT NULL,
CONSTRAINT pk_appointment_stylist_service PRIMARY KEY (AppointmentId, ServiceStylistId),
CONSTRAINT fk_appointment_stylist_service_appointment FOREIGN KEY (AppointmentId) REFERENCES Appointment(Id),
CONSTRAINT fk_appointment_stylist_service_service_stylist FOREIGN KEY (ServiceStylistId) REFERENCES ServiceStylist(Id),
);


CREATE TABLE ReviewStylistClient(
Id INT NOT NULL IDENTITY,
StylistId UNIQUEIDENTIFIER NOT NULL,
ClientId UNIQUEIDENTIFIER NOT NULL,
[Text] NVARCHAR(1000),
Score INT NOT NULL,
CONSTRAINT fk_review_stylist_client PRIMARY KEY (Id),
CONSTRAINT fk_review_stylist_client_stylist FOREIGN KEY (StylistId) REFERENCES Stylist(Id),
CONSTRAINT fk_review_stylist_client_client FOREIGN KEY (ClientId) REFERENCES [User](Id)
);

CREATE TABLE ReviewClientStylist(
Id INT NOT NULL IDENTITY,
ClientId UNIQUEIDENTIFIER NOT NULL,
StylistId UNIQUEIDENTIFIER NOT NULL,
[Text] NVARCHAR(1000),
Score INT NOT NULL,
CONSTRAINT fk_review_client_stylist PRIMARY KEY (Id),
CONSTRAINT fk_review_client_stylist_client FOREIGN KEY (ClientId) REFERENCES [User](Id),
CONSTRAINT fk_review_client_stylist_stylist FOREIGN KEY (StylistId) REFERENCES Stylist(Id) 
);

CREATE TABLE ReviewClientSalon(
Id INT NOT NULL IDENTITY,
ClientId UNIQUEIDENTIFIER NOT NULL,
SalonId INT NOT NULL,
[Text] NVARCHAR(1000),
Score INT NOT NULL,
CONSTRAINT fk_review_client_salon PRIMARY KEY (Id),
CONSTRAINT fk_review_client_salon_client FOREIGN KEY (ClientId) REFERENCES [User](Id),
CONSTRAINT fk_review_client_salon_salon FOREIGN KEY (SalonId) REFERENCES Salon(Id) 
);


CREATE TABLE [Notification] (
Id INT NOT NULL IDENTITY,
ReceiverId UNIQUEIDENTIFIER NOT NULL,
[Text] NVARCHAR(100) NOT NULL,
SendDate DATETIME NOT NULL,
ReadDate DATETIME,
CONSTRAINT pk_notification PRIMARY KEY (Id), 
CONSTRAINT fk_notification_receiver FOREIGN KEY (ReceiverId) REFERENCES [User](Id)
);



INSERT INTO UserStatus(Id, [Name])v
VALUES (1, 'PendingApproval'),
(2, 'Active'),
(3, 'Inactive');

INSERT INTO SalonStatus(Id, [Name])
VALUES (1, 'PendingApproval'),
(2, 'Approved');

create or alter view ServiceDetailsVw as
select 
	salon.Id SalonId
	, salon.[Name] SalonName
	, salon.[Address] 
	, city.[Name] CityName
	, salon.CityId CityId
	, county.[Name] CountyName
	, salon.CountyId CountyId
	, [service].Id ServiceId
	, [service].[Name] ServiceName
	, stylist.Id StylistId
	, concat([user].FirstName, concat(' ', [user].LastName)) StylistName
	from dbo.Stylist stylist
	join dbo.[User] [user] on stylist.UserId = [user].Id
	join dbo.Salon salon on stylist.SalonId = salon.Id 
	join dbo.ServiceStylist ss on stylist.Id = ss.StylistId 
	join dbo.[Service] [service] on ss.ServiceId = [service].id
	join dbo.City city on salon.CityId = city.Id
	join dbo.County county on salon.CountyId = county.Id

  ALTER TABLE [ReviewClientSalon] ADD CONSTRAINT review_client_salon_uq UNIQUE (ClientId, SalonId);
  ALTER TABLE [ReviewClientStylist] ADD CONSTRAINT review_client_stylist_uq UNIQUE (ClientId, StylistId);
  ALTER TABLE [ReviewStylistClient] ADD CONSTRAINT review_stylist_client_uq UNIQUE (StylistId, ClientId);

  
  ALTER TABLE Salon ADD Longitude FLOAT ;
  ALTER TABLE Salon ADD Latitude FLOAT ;


