CREATE DATABASE cardgame ENCODING 'UTF-8';
\c cardgame; 

Create table if not exists users
(
LoginName Varchar primary key,
Passwort Varchar not null,
UserName Varchar,
bio Varchar,
Image Varchar,
Coins varchar default 20
);
Create table if not exists Scoreboard(
    LoginName_fk Varchar primary key,
    win varchar default 0,
    tie varchar default 0,
    lose varchar default 0 ,
    elo varchar default 100,
    FOREIGN KEY(LoginName_fk) REFERENCES users(LoginName) ON DELETE CASCADE
);
Create table if not exists Card
(
    CardID Varchar primary key,
    Cardtype Varchar,
    CardName Varchar,
    BaseDamage varchar,
    CardElement Varchar,
    CardStyle Varchar
);
Create table if not exists UserhasCardsinDeck(
    CardId_fk varchar,
    LoginName_fk varchar,
    FOREIGN KEY(CardId_fk) REFERENCES Card(CardID) ON DELETE CASCADE,
    FOREIGN KEY(LoginName_fk) REFERENCES users(LoginName) ON DELETE CASCADE,
    PRIMARY KEY (CardId_fk,LoginName_fk)
);
Create table if not exists UserhasCardsinStack(
    CardId_fk varchar,
    LoginName_fk varchar,
    CurrentlyTraded bool default false,
    FOREIGN KEY(CardId_fk) REFERENCES Card(CardID) ON DELETE CASCADE,
    FOREIGN KEY(LoginName_fk) REFERENCES users(LoginName) ON DELETE CASCADE,
    PRIMARY KEY (CardId_fk,LoginName_fk)
);
Create table if not exists Packages
(
    PackageId serial primary Key,
    StringCards varchar
);
Create table if not exists Trading(
    TradeID varchar primary key,
    CardToTradeID_fk varchar unique,
    OriginalOwner_fk varchar,
    typeToTrade varchar,
    minDamage varchar,
    TimestampOffer timestamp default now(),
    foreign key(CardToTradeID_fk) references Card(CardID),
    foreign key(OriginalOwner_fk) references users(loginname)
);