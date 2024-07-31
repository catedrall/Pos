create database Consultorio
go 
use Consultorio

create table Consulta
(
	Id int primary key identity(1,1),
	PacienteId int not null,
	MedicoId int not null,
	DataAgendamento datetime not null,
	Ativo bit not null
)

create table Medico
(
	Id int primary key identity(1,1),
	Nome varchar(100) not null,
	Email varchar(100) not null,
	CRM varchar(100) not null,
	Especialidade varchar(100) not null,
	DataNascimento datetime not null,
	Ativo bit not null
)


create table Paciente
(
	Id int primary key identity(1,1),
	Nome varchar(100) not null,
	Email varchar(100) not null,
	DataNascimento datetime not null,
	Ativo bit 
)
alter table Paciente add constraint p_ativo default 1 for ativo
GO
drop table  [Login]

create table [Login]
(
	Id int primary key identity(1,1),
	Email varchar(100),
	Senha varchar(100),
	Ativo bit,
	Medico bit,
	DataCadastro datetime null,
	DataUltimoLogin datetime null
)
alter table Paciente add constraint P_ativo default 1 for ativo
alter table [Login] add constraint L_medico default 1 for Medico
alter table [Login] add constraint L_Ativo default 1 for Ativo
alter table [Login] add constraint L_DataCriacao default 1 for DataCriacao
alter table [Login] add constraint L_DataUltimoLogin default 1 for DataUltimoLogin

