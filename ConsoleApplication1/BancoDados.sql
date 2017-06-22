CREATE TABLE Player(
	ID								BIGINT NOT NULL PRIMARY KEY,
	Nome	  						VARCHAR(100),
	Level	  						BIGINT,
	PontoArena						BIGINT,
	Status							VARCHAR(1)
);
GO

CREATE TABLE PlayerDefesas(
	ID								BIGINT NOT NULL PRIMARY KEY IDENTITY,
	IdPlayer	  					BIGINT,
	NomeOponente					VARCHAR(100),
	NomeGuilda						VARCHAR(100),
	Vitoria							int,	
	DataHora						DATETIME,
	FOREIGN KEY (IdPlayer)			REFERENCES Player(ID)
);
GO

CREATE TABLE Batalhas(
	ID								BIGINT NOT NULL PRIMARY KEY,	
	Guilda  						VARCHAR(100),
	Life							BIGINT,
	Data							Datetime,
	PontuacaoOponente				BIGINT,
	PontuacaoGuild					BIGINT,
	RankGuild						BIGINT
);
GO

CREATE TABLE PlayerStatus(
	ID								BIGINT NOT NULL PRIMARY KEY IDENTITY,
	IdPlayer	  					BIGINT,
	IdBatalha  						BIGINT,
	Status							VARCHAR(1),
	FOREIGN KEY (IdPlayer)			REFERENCES Player(ID),
	FOREIGN KEY (IdBatalha)			REFERENCES Batalhas(ID),
);
GO

CREATE TABLE PlayerOponente(
	ID								BIGINT NOT NULL PRIMARY KEY,
	CodGuilda	  					BIGINT,
	Nome	  						VARCHAR(100),
	Bonus							INT,
	FOREIGN KEY (CodGuilda)			REFERENCES Batalhas(ID),
);
GO

CREATE TABLE Lutas(
	ID								BIGINT NOT NULL PRIMARY KEY IDENTITY,	
	CodBatalhas						BIGINT,
	CodPlayer  						BIGINT,
	CodPlayerOponente				BIGINT,	
	Vitoria							int,
	ValorBarra						BIGINT,
	DataHora						DATETIME,
	MomentoVitoria					VARCHAR(100),
	FOREIGN KEY (CodBatalhas)		REFERENCES Batalhas(ID),
	FOREIGN KEY (CodPlayer)			REFERENCES Player(ID),
	FOREIGN KEY (CodPlayerOponente)	REFERENCES PlayerOponente(ID)
);
GO