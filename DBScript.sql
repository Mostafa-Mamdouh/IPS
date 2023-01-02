


insert into Country  values ('Egypt')

INSERT INTO City(Name,CountryId) VALUES
('Cairo',(select Id from Country where Name='Egypt')),
('Giza',(select Id from Country where Name='Egypt')),
('Alexandria',(select Id from Country where Name='Egypt')),
('Dakahlia',(select Id from Country where Name='Egypt')),
('Red Sea',(select Id from Country where Name='Egypt')),
('Beheira',(select Id from Country where Name='Egypt')),
('Fayoum',(select Id from Country where Name='Egypt')),
('Gharbiya',(select Id from Country where Name='Egypt')),
('Ismailia',(select Id from Country where Name='Egypt')),
( 'Menofia',(select Id from Country where Name='Egypt')),
('Minya',(select Id from Country where Name='Egypt')),
('Qaliubiya',(select Id from Country where Name='Egypt')),
('New Valley',(select Id from Country where Name='Egypt')),
('Suez',(select Id from Country where Name='Egypt')),
('Aswan',(select Id from Country where Name='Egypt')),
('Assiut',(select Id from Country where Name='Egypt')),
('Beni Suef',(select Id from Country where Name='Egypt')),
('Port Said',(select Id from Country where Name='Egypt')),
('Damietta',(select Id from Country where Name='Egypt')),
('Sharkia',(select Id from Country where Name='Egypt')),
('South Sinai',(select Id from Country where Name='Egypt')),
('Kafr Al sheikh',(select Id from Country where Name='Egypt')),
('Matrouh',(select Id from Country where Name='Egypt')),
('Luxor',(select Id from Country where Name='Egypt')),
('North Sinai',(select Id from Country where Name='Egypt')),
('Sohag',(select Id from Country where Name='Egypt'));


insert into Category (Name,Deleted) values
('Card Issuing',0),
('Stickers & Labels',0),
('Access Control & Time Attendance',0),
('Access Automation ',0),
('CCTV',0),
('Undefined',0);

insert into Supplier (Name,CityId,RepresentativeName,MobileNumber,CreateUserId,UpdateUserId,CreateDate,UpdateDate,Deleted)
values('Undefined',1,'Undefined','1234679',1,1,getdate(),getdate(),0)

insert into Client(Name,CityId,RepresentativeName,MobileNumber,CreateUserId,UpdateUserId,CreateDate,UpdateDate,Deleted)
values('Undefined',1,'Undefined','1234679',1,1,getdate(),getdate(),0)

insert into ExpenseType (Name,Deleted) values ('Salaries',0);

SET IDENTITY_INSERT Credit ON 
insert into Credit (Id,BankCredit,CashCredit,[Year],CreateDate,CreateUserId,UpdateDate,UpdateUserId,Deleted) values
(1,0,0,2021,getdate(),(select top 1 Id from AspNetUsers),GETDATE(),(select top 1 Id from AspNetUsers),0)
SET IDENTITY_INSERT Credit off 







