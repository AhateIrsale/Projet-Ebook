create database Projet_EBOOK
use Projet_EBOOK


create table Livre 
(
ID_Livre int primary key,
Titre nvarchar(100),
Auteur nvarchar(100),
Description nvarchar(max),
Date_Publication dateTime,
Date_Edition dateTime,
ID_Categorie int foreign key references Categorie(ID_Categorie),
Image nvarchar(max),
PDF nvarchar(max)
)

create table Categorie(
ID_Categorie int primary Key ,
Nom_Categorie nvarchar(100),
Id_Image nvarchar(max)
)

Create table Admin(
Nom nvarchar(100),
Prenom nvarchar(100),
Email nvarchar(300),
Password nvarchar(100),
constraint pk_admin primary key(Email)
)
create table Utilisateur(
Nom nvarchar(200),
Prenom nvarchar(200),
Email  nvarchar(300),
Password nvarchar(100),
constraint pk_utilisateur primary key(Email)
)

CREATE TABLE conctact(
    id int IDENTITY(1,1) NOT NULL,
    Nom varchar(50)  NULL,
    email varchar(50)  NULL,
    Tel varchar(50)  NULL,
    contenu varchar(max)  NULL)