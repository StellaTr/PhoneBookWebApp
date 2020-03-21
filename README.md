# PhoneBookWebApp
An application for adding Contact and their Phone Number
Developed with C#/.NET Core and React

****************************************************************************
Database ::
The database used was SQL Server Express 2017  (Windows Authentication enabled)

The scripts to create the tables ::

****************************************************************************
create table Contact(
	ContactId INT PRIMARY KEY IDENTITY (1, 1),
	FirstName NVARCHAR(40) NOT NULL,
	LastName NVARCHAR(40) NOT NULL
);

create table ContactPhone(
	ContactPhoneId INT PRIMARY KEY IDENTITY (1, 1),
	CountryCode VARCHAR(10) NOT NULL,
	AreaCode VARCHAR(10) NOT NULL,
	PhoneNumber NVARCHAR(20) NOT NULL,
	ContactId INT NOT NULL,
	FOREIGN KEY (ContactId) REFERENCES Contact (ContactId)
);

ALTER TABLE Contact
ADD CONSTRAINT UC_Contact UNIQUE (FirstName,LastName);

ALTER TABLE ContactPhone
ADD CONSTRAINT UC_ContactPhone UNIQUE (CountryCode,AreaCode,PhoneNumber);
****************************************************************************

Configure database in project ::
The database is configured in the PhoneBookWebApp project, in the appsettings.json file. Modify the DefaultConnection accordingly.<br/>

Current settings:<br/> 
"DefaultConnection": "Server=.\\SQLExpress;Database=PhoneBookNewDB;Trusted_Connection=True;MultipleActiveResultSets=true"<br/>
Database Name: PhoneBookNewDB<br/>
Windows Authentication enabled

PhoneBookWebApp ::
Based on launchSettings.json the application is currently configured to listen to <http://localhost:54083>
...\PhoneBookWebApp\PhoneBookWebApp\Properties\launchSettings.json

Client app ::
The client app is configured to run on http://localhost:3000 (default). 
If you want to change address, modify the ClientApp section in appsettings.json in PhoneWebApp project

Depending on where your PhoneBookWebApp web api is configured to listen to, you will need to change the *const baseURL* variable in the Utils.js component<br/>
...\PhoneBookWebApp\PhoneBookWebApp\ClientApp\src\components\Utils.js <br/>
Current settings :: const baseURL = "http://localhost:54083/api/Contacts";
