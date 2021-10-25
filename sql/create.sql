create database school encoding 'utf-8';
\c school;


CREATE TYPE Gender AS ENUM ('male','female');
CREATE  TABLE TEACHERS
(
    ID            VARCHAR(24)        NOT NULL    PRIMARY KEY,
    NAME         TEXT,
    FIRSTNAME    TEXT,
    GENDER        Gender,
    BDATE        TIMESTAMP,
    HDATE        TIMESTAMP,
    SALARY        INTEGER
);
CREATE TABLE COURSES
(
    ID            VARCHAR(24)    NOT NULL    PRIMARY KEY,
    HACTIVE     INTEGER        NOT NULL     DEFAULT 0,
    NAME         TEXT,
    KTEACHER     VARCHAR(24)    NOT NULL    REFERENCES TEACHERS(ID)
);
CREATE TABLE CLASSES
(
    ID            VARCHAR(24)    NOT NULL    PRIMARY KEY,
    NAME        TEXT,
    KTEACHER    VARCHAR(24)    NOT NULL    REFERENCES TEACHERS(ID)
);
CREATE TABLE STUDENTS
(
    ID            VARCHAR(24)    NOT NULL    PRIMARY KEY,
    NAME         TEXT,
    FIRSTNAME    TEXT,
    GENDER        Gender,
    BDATE        TIMESTAMP,
    KCLASS        VARCHAR(24)    REFERENCES CLASSES(ID),
    GRADE        INTEGER
);
CREATE TABLE STUDENT_COURSES
(
    KSTUDENT VARCHAR(24) NOT NULL REFERENCES STUDENTS (ID),
    KCOURSE  VARCHAR(24) NOT NULL REFERENCES COURSES (ID)
);


/*drop table STUDENT_COURSES;
drop table STUDENTS;
drop table CLASSES;
drop table COURSES;
drop table TEACHERS;
drop type Gender;*/