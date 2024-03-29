------TABLESPACE FOR COURSEWORK------

CREATE TABLESPACE TS_COURSEWORK
DATAFILE 'C:\OracleDB\CW\TS_COURSEWORK.DBF'
SIZE 20M
AUTOEXTEND ON NEXT 1M
MAXSIZE 2048M;

-------TEMPORARY TABLESPACE FOR COURSEWORK--------

CREATE TEMPORARY TABLESPACE TS_COURSEWORK_TEMP
TEMPFILE 'C:\OracleDB\CW\TS_COURSEWORK_TEMP.DBF'
SIZE 10M
AUTOEXTEND ON NEXT 1M
MAXSIZE 2048M;

----------ROLE FOR MANAGERS COURSEWORK---------

alter session set "_ORACLE_SCRIPT"=true;
--drop role RLMANAGERCORE;
CREATE ROLE RLMANAGERCORE;
--
----------PERMISSIONS FOR MANAGERS-----------
--
GRANT CREATE SESSION,
	  CREATE TABLE,
	  CREATE VIEW,
	  CREATE PROCEDURE,
	  CREATE TRIGGER TO RLMANAGERCORE;
--
GRANT ALL ON TICKETS TO RLMANAGERCORE;
--
GRANT ALL ON TRAINS TO RLMANAGERCORE;
--
GRANT ALL ON VANS TO RLMANAGERCORE;
--
GRANT ALL ON SCHEDULE TO RLMANAGERCORE;
--
GRANT ALL ON PASSENGERS TO RLMANAGERCORE;

GRANT ALL ON PAYMENTS TO RLMANAGERCORE;

GRANT ALL ON STATIONS_ROUTES TO RLMANAGERCORE;

GRANT ALL ON STATIONS TO RLMANAGERCORE;

GRANT ALL ON ROUTES TO RLMANAGERCORE;

CREATE USER MANAGER
IDENTIFIED BY MANAGER_PASS
TEMPORARY TABLESPACE TS_COURSEWORK_TEMP
DEFAULT TABLESPACE TS_COURSEWORK
QUOTA UNLIMITED ON TS_COURSEWORK
ACCOUNT UNLOCK;

GRANT RLMANAGERCORE TO MANAGER;

ALTER SESSION SET "_oracle_script" = TRUE;

--------PROFILE FOR MANAGERS------
CREATE PROFILE PMANAGERCORE LIMIT
FAILED_LOGIN_ATTEMPTS 3
PASSWORD_LIFE_TIME UNLIMITED
PASSWORD_GRACE_TIME UNLIMITED
PASSWORD_LOCK_TIME UNLIMITED;

ALTER USER MANAGER PROFILE PMANAGERCORE;

--------ROLE FOR USERS-------------
-- alter session set "_ORACLE_SCRIPT"=true;
--DROP ROLE RLUSERCORE;
CREATE ROLE RLUSERCORE;

------PERMISSIONS FOR USERS---------

GRANT CREATE SESSION TO RLUSERCORE;
--
GRANT SELECT, DELETE ON TAKE_TICKET TO RLUSERCORE;

--
GRANT
    SELECT,
    INSERT,
    UPDATE ON MANAGER.PASSENGERS TO RLUSERCORE;
--
GRANT
    SELECT,
    INSERT,
    UPDATE ON MANAGER.TICKETS TO RLUSERCORE;
--
GRANT
    SELECT ON MANAGER.SCHEDULE TO RLUSERCORE;

GRANT select ON MANAGER.VANS To RLUSERCORE;

GRANT SELECT ON MANAGER.TAKE_SCHEDULE TO RLUSERCORE;

GRANT SELECT ON MANAGER.TAKE_SCHEDULE_USER TO RLUSERCORE;

GRANT
    SELECT,
    INSERT ON MANAGER.PAYMENTS TO RLUSERCORE;

GRANT SELECT ON MANAGER.STATIONS TO RLUSERCORE;

GRANT SELECT ON MANAGER.STATIONS_ROUTES TO RLUSERCORE;

GRANT SELECT ON MANAGER.ROUTES TO RLUSERCORE;

GRANT SELECT ON MANAGER.TRAINS TO RLUSERCORE;

GRANT SELECT, INSERT, UPDATE ON MANAGER.TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.INSERT_TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.UPDATE_TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.DELETE_TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.INSERT_PAYMENTS TO RLUSERCORE;

----???? БОТУ ПРОВЕРКИ ОПЛАТЫ ????
GRANT EXECUTE ON MANAGER.UPDATE_PAYMENTS TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.INSERT_PASSENGERS TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.UPDATE_PASSENGERS TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.DELETE_PASSENGERS TO RLUSERCORE;

------CREATE USER FOR USERS-----------

CREATE USER USERS
IDENTIFIED BY USERS_PASS
TEMPORARY TABLESPACE TS_COURSEWORK_TEMP
DEFAULT TABLESPACE TS_COURSEWORK
QUOTA UNLIMITED ON TS_COURSEWORK
ACCOUNT UNLOCK;

GRANT RLUSERCORE TO USERS;

------PROFILE FOR USERS--------
CREATE PROFILE PUSERCORE LIMIT
FAILED_LOGIN_ATTEMPTS 5
PASSWORD_LIFE_TIME 90
PASSWORD_GRACE_TIME 7;

ALTER USER USERS PROFILE PUSERCORE;

--------CREATE TABLE SCHEDULE-------
CREATE TABLE SCHEDULE (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY  PRIMARY KEY,
    ID_TRAIN NUMBER(14, 0) NOT NULL,
    "DATE" DATE NOT NULL,
    ROUTE NUMBER(14,0) NOT NULL,
    FREQUENCY NUMBER(1) CHECK (FREQUENCY IN (1,2,3,4)) NOT NULL,
    CONSTRAINT NUMBER_OF_TRAIN_FK FOREIGN KEY (ID_TRAIN) REFERENCES TRAINS("ID"),
    CONSTRAINT SROUTE_FK FOREIGN KEY (ROUTE) REFERENCES ROUTES("ID")
);

CREATE INDEX id_train_date_idx ON schedule (id_train, 'date');

--------View for Schedule---------
CREATE OR REPLACE VIEW TAKE_SCHEDULE AS
    SELECT SCHEDULE."ID", ID_TRAIN, TRAINS.CATEGORY_OF_TRAIN,
           ROUTES.DEPARTURE_POINT, ROUTES.ARRIVAL_POINT,
            ROUTES.DISTANCE, ROUTES.DURATION,
           "DATE", FREQUENCY, TRAINS.IS_FOR_PASSENGERS
    FROM SCHEDULE
        JOIN TRAINS ON SCHEDULE.ID_TRAIN = TRAINS.ID
        JOIN ROUTES ON SCHEDULE.ROUTE = ROUTES.ID;

-----------VARRAY FOR VANS-------------

-- DROP TYPE VANS_COMPOSITION;
-- CREATE OR REPLACE TYPE VANS_COMPOSITION AS VARRAY(15) OF NUMBER(14,0);

----------CREATE TABLE TRAINS----------
CREATE TABLE TRAINS (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY  PRIMARY KEY,
    CATEGORY_OF_TRAIN NVARCHAR2(15) NOT NULL,
    IS_FOR_PASSENGERS Number(1) CHECK ( IS_FOR_PASSENGERS IN (0,1)) NOT NULL,
    VANS Nvarchar2(2000) NOT NULL,
    COUNT_OF_VANS NUMBER(2) NOT NULL,
    PARKING_TIME NUMBER(8,0) NOT NULL
);

-- ----------Trigger for TRAINS--------------
-- CREATE OR REPLACE TRIGGER TRAINS_INSERT_UPDATE_TRIGGER
-- BEFORE INSERT OR UPDATE ON TRAINS
-- FOR EACH ROW
-- DECLARE
--   vans_in VANS_COMPOSITION := VANS_COMPOSITION();
--   vans_out VANS_COMPOSITION := VANS_COMPOSITION();
--   vansfree NUMBER(1, 0);
-- BEGIN
--   IF INSERTING THEN
--     vans_in := :NEW.VANS;
--     vans_out := :NEW.VANS;
--
--     FOR I IN vans_in.FIRST .. vans_in.LAST LOOP
--       SELECT IS_FREE INTO vansfree FROM VANS WHERE ID = vans_in(I);
--       IF vansfree = 0 THEN
--             vans_out := DELETE_ENTRY(vans_out, vans_in(I));
--       ELSE
--         UPDATE VANS SET IS_FREE = 0 WHERE ID = vans_in(I);
--       END IF;
--     END LOOP;
--
--     :NEW.COUNT_OF_VANS := vans_out.COUNT;
--     :NEW.VANS := vans_out;
--
--   ELSIF UPDATING('VANS') THEN
--     vans_in := :OLD.VANS;
--
--     FOR I IN vans_in.FIRST .. vans_in.LAST LOOP
--       SELECT IS_FREE INTO vansfree FROM VANS WHERE ID = vans_in(I);
--       UPDATE VANS SET IS_FREE = 1 WHERE ID = vans_in(I);
--     END LOOP;
--
--     vans_in := :NEW.VANS;
--
--     FOR I IN vans_in.FIRST .. vans_in.LAST LOOP
--       SELECT IS_FREE INTO vansfree FROM VANS WHERE ID = vans_in(I);
--       IF vansfree = 0 THEN
--             vans_out := DELETE_ENTRY(vans_out, vans_in(I));
--       ELSE
--         UPDATE VANS SET IS_FREE = 0 WHERE ID = vans_in(I);
--       END IF;
--     END LOOP;
--
--     :NEW.VANS := vans_out;
--     :NEW.COUNT_OF_VANS := vans_out.COUNT;
--   END IF;
-- END;

----------CREATE TABLE VANS-------------

CREATE TABLE VANS (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY  PRIMARY KEY,
    "TYPE" NVARCHAR2(15) NOT NULL,
    "CAPACITY" NUMBER(4,0) NOT NULL,
    IS_FREE NUMBER(1,0) CHECK ( IS_FREE IN (0, 1) ) NOT NULL
);

CREATE INDEX type_is_free_idx ON vans (type, is_free);


CREATE TABLE PASSENGERS (
  "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
   FULL_NAME NVARCHAR2(50) NOT NULL,
   PASSPORT NVARCHAR2(9) NOT NULL,
   BENEFITS NUMBER(3,0) CHECK (BENEFITS < 101) NOT NULL
);
---------CREATE TABLE TICKET-------------
CREATE TABLE TICKETS (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ID_PASSENGER NUMBER(14,0) NOT NULL,
    ID_TRAIN NUMBER(14,0) NOT NULL,
    ID_VAN NUMBER(14,0) NOT NULL,
    SEAT_NUMBER NUMBER(4,0) NOT NULL,
    FROM_WHERE NUMBER(14, 0) NOT NULL,
    TO_WHERE NUMBER(14, 0) NOT NULL,
    "DATE" DATE NOT NULL,
    COST NUMBER(8,0) NOT NULL,
    CONSTRAINT ID_PASSENGER_FK FOREIGN KEY (ID_PASSENGER) REFERENCES PASSENGERS("ID"),
    CONSTRAINT ID_TRAIN_FK FOREIGN KEY (ID_TRAIN) REFERENCES  TRAINS("ID"),
    CONSTRAINT ID_VAN_FK FOREIGN KEY (ID_VAN) REFERENCES VANS("ID"),
    CONSTRAINT FROM_WHERE_FK FOREIGN KEY (FROM_WHERE) REFERENCES STATIONS("ID"),
    CONSTRAINT TO_WHERE_FK FOREIGN KEY (TO_WHERE) REFERENCES STATIONS("ID")
);

CREATE INDEX DATE_SORT_INDEX ON TICKETS("DATE" DESC);

---------VIEW FOR TICKETS------------
CREATE OR REPLACE VIEW TAKE_TICKET AS
    SELECT TICKETS.ID, ID_PASSENGER, ID_TRAIN, ID_VAN, SEAT_NUMBER, FROM_WHERE, TO_WHERE, "DATE", COST, DATE_PAY, STATUS FROM TICKETS
        JOIN PAYMENTS ON TICKETS.ID = PAYMENTS.ID_TICKET;

SELECT * FROM TAKE_TICKET;

----------TRIGGER FOR TICKETS--------
-- CREATE OR REPLACE TRIGGER increase_van_capacity
-- AFTER INSERT OR UPDATE ON TICKETS
-- BEGIN
--   UPDATE_VANS_CAPACITY();
-- END;

-- CREATE OR REPLACE TRIGGER CHECK_VAN
-- BEFORE INSERT OR UPDATE ON TICKETS
-- FOR EACH ROW
--     DECLARE VANS VANS_COMPOSITION := VANS_COMPOSITION();
--             BOOL NUMBER(1,0) := 0;
-- BEGIN
--     SELECT VANS INTO VANS FROM TRAINS WHERE ID = :NEW.ID_TRAIN;
--
--     FOR I IN VANS.FIRST .. VANS.LAST LOOP
--       IF VANS(I) = :NEW.ID_VAN THEN
--             BOOL := 1;
--       END IF;
--     END LOOP;
--
--     IF BOOL = 0 THEN
--         :NEW.ID_VAN := NULL;
--         RAISE_APPLICATION_ERROR(-20001, 'VAN NOT IN TRAIN');
--     end if;
-- end;
--
-- CREATE OR REPLACE TRIGGER CHECK_SEAT
-- BEFORE INSERT OR UPDATE ON TICKETS
-- FOR EACH ROW
--     DECLARE FREE NUMBER;
-- BEGIN
--     IF :NEW.SEAT_NUMBER != :OLD.SEAT_NUMBER OR INSERTING THEN
--
--     SELECT COUNT(*) INTO FREE FROM TICKETS WHERE SEAT_NUMBER = :NEW.SEAT_NUMBER AND ID_VAN = :NEW.ID_VAN;
--
--     IF FREE > 0 THEN
--         RAISE_APPLICATION_ERROR(-20001,'THIS PLACE ALREADY USED');
--     end if;
--     end if;
-- end;

CREATE OR REPLACE TRIGGER CHECK_ROUTE
BEFORE INSERT OR UPDATE ON TICKETS
FOR EACH ROW
    DECLARE FROM_BOOL NUMBER;
            TO_BOOL NUMBER;
BEGIN
    SELECT COUNT(*) INTO FROM_BOOL FROM ROUTES
        JOIN STATIONS_ROUTES ON ROUTES.ID = STATIONS_ROUTES.ROUTE_ID
        JOIN SCHEDULE S on ROUTES.ID = S.ROUTE
                                   WHERE STATION_ID = :NEW.FROM_WHERE AND S.ID_TRAIN = :NEW.ID_TRAIN;

    SELECT COUNT(*) INTO TO_BOOL FROM ROUTES
        JOIN STATIONS_ROUTES ON ROUTES.ID = STATIONS_ROUTES.ROUTE_ID
        JOIN SCHEDULE S on ROUTES.ID = S.ROUTE
                                   WHERE STATION_ID = :NEW.TO_WHERE AND S.ID_TRAIN = :NEW.ID_TRAIN;

    IF FROM_BOOL = 0 OR TO_BOOL = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'DOES NOT EXIST THIS ROUTE');
    end if;
end;

--------trigger for PAYMENTS---------
CREATE OR REPLACE TRIGGER update_van_capacity
BEFORE INSERT OR UPDATE ON PAYMENTS
FOR EACH ROW
WHEN (NEW.STATUS = 'S')
DECLARE
  VAN_CAPACITY NUMBER;
BEGIN
    -- Получаем вместимость вагона из таблицы VANS
    SELECT CAPACITY INTO VAN_CAPACITY
    FROM VANS
    WHERE ID = (SELECT ID_VAN FROM TICKETS WHERE ID = :NEW.ID_TICKET);

    -- Проверяем, достигнута ли максимальная вместимость вагона
    IF VAN_CAPACITY = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'The capacity of the van is zero. Cannot insert a new row into the PAYMENTS table.');
    END IF;

    -- Уменьшаем значение столбца CAPACITY в таблице VANS
    UPDATE VANS
    SET CAPACITY = CAPACITY - 1
    WHERE ID = (SELECT ID_VAN FROM TICKETS WHERE ID = :NEW.ID_TICKET);
END;

--------CREATE TABLE FOR PAYMENTS----------
CREATE TABLE PAYMENTS (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY  PRIMARY KEY,
    ID_TICKET NUMBER(14,0) NOT NULL,
    DATE_PAY DATE NOT NULL,
    STATUS CHAR(1) CHECK ( STATUS IN ('S', 'R', 'W')) NOT NULL,
    CONSTRAINT ID_TICKET_FK FOREIGN KEY  (ID_TICKET) REFERENCES TICKETS("ID")
);

---------CREATE TABLE ROUTES---------------
CREATE TABLE ROUTES (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY  PRIMARY KEY,
    DEPARTURE_POINT NUMBER(14,0) NOT NULL,
    ARRIVAL_POINT NUMBER(14,0) NOT NULL,
    DISTANCE NUMBER NOT NULL,
    DURATION NUMBER NOT NULL,
    CONSTRAINT DEPARTURE_POINT_FK FOREIGN KEY (DEPARTURE_POINT) REFERENCES STATIONS(ID),
    CONSTRAINT ARRIVAL_POINT_FK FOREIGN KEY (ARRIVAL_POINT) REFERENCES STATIONS(ID)
);

-----------CREATE TABLE STATIONS-----------
CREATE TABLE STATIONS (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY  PRIMARY KEY,
    STATION_NAME NVARCHAR2(30) NOT NULL,
    CITY NVARCHAR2(30) NOT NULL,
    STATE NVARCHAR2(30) NOT NULL,
    COUNTRY NVARCHAR2(30) NOT NULL
);

---------CREATE TABLES STATIONS_ROUTES---------
CREATE TABLE STATIONS_ROUTES (
    "ID" NUMBER(14,0) GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ROUTE_ID NUMBER(14,0) NOT NULL,
    STATION_ID NUMBER(14,0) NOT NULL,
    STATION_ORDER NUMBER NOT NULL,
    CONSTRAINT STATION_ID_FK FOREIGN KEY (STATION_ID) REFERENCES STATIONS(ID),
    CONSTRAINT ROUTE_FK FOREIGN KEY (ROUTE_ID) REFERENCES ROUTES(ID)
);

--------PROCEDURES---------
CREATE OR REPLACE PROCEDURE UPDATE_VANS_CAPACITY
AS BEGIN
DECLARE
  CURSOR cur IS SELECT ID_VAN FROM TICKETS WHERE TICKETS."DATE" < SYSDATE;
  TYPE id_tab_type IS TABLE OF TICKETS.ID_VAN%TYPE INDEX BY PLS_INTEGER;
  id_tab id_tab_type;
BEGIN
  OPEN cur;
  LOOP
    FETCH cur BULK COLLECT INTO id_tab LIMIT 1000;
    EXIT WHEN id_tab.COUNT = 0;
    FORALL i IN 1..id_tab.COUNT
      UPDATE VANS SET CAPACITY = CAPACITY + 1
                    WHERE ID = id_tab(i);
  END LOOP;
  CLOSE cur;
END;
end;


CREATE OR REPLACE PROCEDURE INSERT_PASSENGERS(
    FULLNAME IN NVARCHAR2,
    PASSPOR IN NVARCHAR2,
    BENEFIT IN NUMBER)
IS
BEGIN
    INSERT INTO MANAGER.PASSENGERS(FULL_NAME, PASSPORT, BENEFITS) VALUES (FULLNAME, PASSPOR, BENEFIT);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_PASSENGERS(
    ID_IN IN NUMBER,
    FULLNAME IN NVARCHAR2,
    PASSPOR IN NVARCHAR2,
    BENEFIT IN NUMBER)
IS
BEGIN
    UPDATE MANAGER.PASSENGERS SET FULL_NAME = FULLNAME, PASSPORT = PASSPOR,
        BENEFITS = BENEFIT WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_PASSENGERS(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.PASSENGERS WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_PAYMENTS(
    IDTICKET IN NUMBER,
    DATEPAY IN DATE,
    STAT IN CHAR)
IS
BEGIN
    INSERT INTO MANAGER.PAYMENTS(ID_TICKET, DATE_PAY, STATUS) VALUES (IDTICKET, DATEPAY, STAT);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
END;

CREATE OR REPLACE PROCEDURE UPDATE_PAYMENTS(
    ID_IN IN NUMBER,
    IDTICKET IN NUMBER,
    DATEPAY IN DATE,
    STAT IN CHAR)
IS
BEGIN
    UPDATE MANAGER.PAYMENTS SET ID_TICKET = IDTICKET, DATE_PAY = DATEPAY,
        STATUS = STAT WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_PAYMENTS(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.PAYMENTS WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_ROUTES(
    DEPARTURE_POINT_ID IN NUMBER,
    ARRIVAL_POINT_ID IN NUMBER,
    DISTANCE_IN IN NUMBER,
    DURATION_IN IN NUMBER)
IS
BEGIN
    INSERT INTO MANAGER.ROUTES(DEPARTURE_POINT, ARRIVAL_POINT, DISTANCE, DURATION)
    VALUES (DEPARTURE_POINT_ID, ARRIVAL_POINT_ID, DISTANCE_IN, DURATION_IN);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_ROUTES(
    ID_IN IN NUMBER,
    DEPARTURE_POINT_ID IN NUMBER,
    ARRIVAL_POINT_ID IN NUMBER,
    DISTANCE_IN IN NUMBER,
    DURATION_IN IN NUMBER)
IS
BEGIN
    UPDATE MANAGER.ROUTES SET DEPARTURE_POINT = DEPARTURE_POINT_ID, ARRIVAL_POINT = ARRIVAL_POINT_ID,
        DISTANCE = DISTANCE_IN, DURATION = DURATION_IN WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_ROUTES(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.ROUTES WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_SCHEDULE(
    ID_TRAIN_IN IN NUMBER,
    DATE_IN IN DATE,
    ROUTE_ID IN NUMBER,
    FREQUENCY_IN IN NUMBER)
IS
BEGIN
    INSERT INTO MANAGER.SCHEDULE(ID_TRAIN, "DATE", ROUTE, FREQUENCY)
    VALUES (ID_TRAIN_IN, DATE_IN, ROUTE_ID, FREQUENCY_IN);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_SCHEDULE(
    ID_IN IN NUMBER,
    ID_TRAIN_IN IN NUMBER,
    DATE_IN IN DATE,
    ROUTE_ID IN NUMBER,
    FREQUENCY_IN IN NUMBER)
IS
BEGIN
    UPDATE MANAGER.SCHEDULE SET ID_TRAIN = ID_TRAIN_IN, "DATE" = DATE_IN,
        ROUTE = ROUTE_ID, FREQUENCY = FREQUENCY_IN WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_SCHEDULE(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.SCHEDULE WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_STATIONS(
    STATION_NAME_IN IN NVARCHAR2,
    CITY_IN IN NVARCHAR2,
    STATE_IN IN NVARCHAR2,
    COUNTRY_IN IN NVARCHAR2)
IS
BEGIN
    INSERT INTO MANAGER.STATIONS(STATION_NAME, CITY, STATE, COUNTRY)
    VALUES (STATION_NAME_IN, CITY_IN, STATE_IN, COUNTRY_IN);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_STATIONS(
    ID_IN IN NUMBER,
    STATION_NAME_IN IN NVARCHAR2,
    CITY_IN IN NVARCHAR2,
    STATE_IN IN NVARCHAR2,
    COUNTRY_IN IN NVARCHAR2)
IS
BEGIN
    UPDATE MANAGER.STATIONS SET STATION_NAME = STATION_NAME_IN, CITY = CITY_IN,
        STATE = STATE_IN, COUNTRY = COUNTRY_IN WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_STATIONS(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.STATIONS WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_STATIONS_ROUTES(
    ROUTE_ID_IN IN NUMBER,
    STATION_ID_IN IN NUMBER,
    STATION_ORDER_IN IN NUMBER)
IS
BEGIN
    INSERT INTO MANAGER.STATIONS_ROUTES(ROUTE_ID, STATION_ID, STATION_ORDER)
    VALUES (ROUTE_ID_IN, STATION_ID_IN, STATION_ORDER_IN);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_STATIONS_ROUTES(
    ID_IN IN NUMBER,
    ROUTE_ID_IN IN NUMBER,
    STATION_ID_IN IN NUMBER,
    STATION_ORDER_IN IN NUMBER)
IS
BEGIN
    UPDATE MANAGER.STATIONS_ROUTES SET ROUTE_ID = ROUTE_ID_IN, STATION_ID = STATION_ID_IN,
        STATION_ORDER = STATION_ORDER_IN WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_STATIONS_ROUTES(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.STATIONS_ROUTES WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_TICKETS(
    ID_PASSENGER_IN IN NUMBER,
    ID_TRAIN_IN IN NUMBER,
    ID_VAN_IN IN NUMBER,
    SEAT_NUMBER_IN IN NUMBER,
    FROM_WHERE_IN IN NUMBER,
    TO_WHERE_IN IN NUMBER,
    DATE_IN IN DATE,
    COST_IN IN NUMBER)
IS
BEGIN
    INSERT INTO MANAGER.TICKETS(ID_PASSENGER, ID_TRAIN, ID_VAN, SEAT_NUMBER, FROM_WHERE, TO_WHERE, "DATE", COST)
    VALUES (ID_PASSENGER_IN, ID_TRAIN_IN, ID_VAN_IN, SEAT_NUMBER_IN, FROM_WHERE_IN, TO_WHERE_IN, DATE_IN, COST_IN);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_TICKETS(
    ID_IN IN NUMBER,
    ID_PASSENGER_IN IN NUMBER,
    ID_TRAIN_IN IN NUMBER,
    ID_VAN_IN IN NUMBER,
    SEAT_NUMBER_IN IN NUMBER,
    FROM_WHERE_IN IN NUMBER,
    TO_WHERE_IN IN NUMBER,
    DATE_IN IN DATE,
    COST_IN IN NUMBER)
IS
BEGIN
    UPDATE MANAGER.TICKETS SET ID_PASSENGER = ID_PASSENGER_IN, ID_TRAIN = ID_TRAIN_IN,
        ID_VAN = ID_VAN_IN, SEAT_NUMBER = SEAT_NUMBER_IN, FROM_WHERE = FROM_WHERE_IN,
        TO_WHERE = TO_WHERE_IN, "DATE" = DATE_IN, COST = COST_IN WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_TICKETS(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.TICKETS WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_TRAINS(
    CATEGORY_OF_TRAIN_IN IN NVARCHAR2,
    IS_FOR_PASSENGERS_IN IN NUMBER,
    VANS_IN IN NVARCHAR2,
    COUNT_OF_VANS_IN IN NUMBER,
    PARKING_TIME_IN IN NUMBER)
IS
BEGIN
    INSERT INTO MANAGER.TRAINS(CATEGORY_OF_TRAIN, IS_FOR_PASSENGERS, VANS, COUNT_OF_VANS, PARKING_TIME)
    VALUES (CATEGORY_OF_TRAIN_IN, IS_FOR_PASSENGERS_IN, VANS_IN, COUNT_OF_VANS_IN, PARKING_TIME_IN);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
CREATE OR REPLACE PROCEDURE UPDATE_TRAINS(
    ID_IN IN NUMBER,
    CATEGORY_OF_TRAIN_IN IN NVARCHAR2,
    IS_FOR_PASSENGERS_IN IN NUMBER,
    VANS_IN IN NVARCHAR2,
    COUNT_OF_VANS_IN IN NUMBER,
    PARKING_TIME_IN IN NUMBER)
IS
BEGIN
    UPDATE MANAGER.TRAINS SET CATEGORY_OF_TRAIN = CATEGORY_OF_TRAIN_IN, IS_FOR_PASSENGERS = IS_FOR_PASSENGERS_IN,
        VANS = VANS_IN, COUNT_OF_VANS = COUNT_OF_VANS_IN, PARKING_TIME = PARKING_TIME_IN WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_TRAINS(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.TRAINS WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_VANS(
    TYPE_IN IN NVARCHAR2,
    CAPACITY_IN IN NUMBER,
    IS_FREE_IN IN NUMBER)
IS
BEGIN
    INSERT INTO MANAGER.VANS("TYPE", "CAPACITY", IS_FREE)
    VALUES (TYPE_IN, CAPACITY_IN, IS_FREE_IN);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_VANS(
    ID_IN IN NUMBER,
    TYPE_IN IN NVARCHAR2,
    CAPACITY_IN IN NUMBER,
    IS_FREE_IN IN NUMBER)
IS
BEGIN
    UPDATE MANAGER.VANS SET "TYPE" = TYPE_IN, CAPACITY = CAPACITY_IN,
        IS_FREE = IS_FREE_IN WHERE ID = ID_IN;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE DELETE_VANS(
    ID_IN NUMBER
)
IS BEGIN
    DELETE FROM MANAGER.VANS WHERE ID = ID_IN;
end;


CREATE OR REPLACE PROCEDURE INSERT_TAKE_TICKET(
    p_id_passenger IN NUMBER,
    p_id_train IN NUMBER,
    p_id_van IN NUMBER,
    p_seat_number IN NUMBER,
    p_from_where IN VARCHAR2,
    p_to_where IN VARCHAR2,
    p_date IN DATE,
    p_cost IN NUMBER,
    p_date_pay IN DATE,
    p_status IN CHAR
)
IS
    ID_TICK NUMBER;
BEGIN
    INSERT INTO MANAGER.TICKETS (ID_PASSENGER, ID_TRAIN, ID_VAN, SEAT_NUMBER, FROM_WHERE, TO_WHERE, "DATE", COST)
    VALUES (p_id_passenger, p_id_train, p_id_van, p_seat_number, p_from_where, p_to_where, p_date, p_cost);
    SELECT ID INTO ID_TICK
    FROM MANAGER.TICKETS
    WHERE ID_PASSENGER = p_id_passenger
      AND ID_TRAIN = p_id_train
      AND ID_VAN = p_id_van
      AND SEAT_NUMBER = p_seat_number
      AND FROM_WHERE = p_from_where
      AND TO_WHERE = p_to_where
      AND "DATE" = p_date
      AND COST = p_cost;
    INSERT INTO MANAGER.PAYMENTS (ID_TICKET, DATE_PAY, STATUS)
    VALUES (ID_TICK, p_date_pay, p_status);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE UPDATE_TAKE_TICKET(
    p_id IN NUMBER,
    p_id_passenger IN NUMBER,
    p_id_train IN NUMBER,
    p_id_van IN NUMBER,
    p_seat_number IN NUMBER,
    p_from_where IN VARCHAR2,
    p_to_where IN VARCHAR2,
    p_date IN DATE,
    p_cost IN NUMBER,
    p_date_pay IN DATE,
    p_status IN VARCHAR2
)
IS
BEGIN
    UPDATE MANAGER.TICKETS SET
        ID_PASSENGER = p_id_passenger,
        ID_TRAIN = p_id_train,
        ID_VAN = p_id_van,
        SEAT_NUMBER = p_seat_number,
        FROM_WHERE = p_from_where,
        TO_WHERE = p_to_where,
        "DATE" = p_date,
        COST = p_cost
    WHERE ID = p_id;

    UPDATE MANAGER.PAYMENTS SET
        DATE_PAY = p_date_pay,
        STATUS = p_status
    WHERE ID_TICKET = p_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
UPDATE TAKE_TICKET SET STATUS = 'W' WHERE ID = 22;
SELECT * FROM TAKE_TICKET;
CREATE OR REPLACE PROCEDURE DELETE_TAKE_TICKET(
    p_id IN NUMBER
    )
IS BEGIN
    DELETE FROM PAYMENTS WHERE ID_TICKET = p_id;
    DELETE FROM TICKETS WHERE ID = p_id;
    COMMIT;
end;

-- CALL UPDATE_VANS(1, 'TYPEEX', 123, 0);

-----------FUNCTIONS----------
-- CREATE OR REPLACE FUNCTION delete_entry(p_record IN VANS_COMPOSITION, p_val IN NUMBER)
-- RETURN VANS_COMPOSITION
-- IS
--    v_ret  VANS_COMPOSITION := VANS_COMPOSITION();
-- BEGIN
--    FOR n IN p_record.FIRST..p_record.LAST LOOP
--       IF p_record(n) != p_val THEN
--         v_ret.EXTEND;
--         v_ret(v_ret.LAST) := p_record(n);
--       END IF;
--    END LOOP;
--    RETURN v_ret;
-- END;

-----------TO JSON-------------
-- CREATE OR REPLACE DIRECTORY utl_dir AS 'D:\Oracle_db\CW\archive_data';
-- GRANT READ, WRITE ON DIRECTORY utl_dir TO public;
-- CREATE OR REPLACE PROCEDURE TO_JSON(return_out out int)
-- IS BEGIN
--     DECLARE
--   output_file UTL_FILE.FILE_TYPE;
--   json_data CLOB;
--     CURSOR vans_cursor IS
--     SELECT JSON_OBJECT(
--              'type' VALUE TYPE,
--              'capacity' VALUE CAPACITY,
--              'is_free' VALUE IS_FREE
--            ) AS json_data
--     FROM VANS
--     WHERE ROWNUM < 10;
-- BEGIN
--
-- output_file := UTL_FILE.FOPEN('UTL_DIR', 'VANS.JSON', 'W');
--
-- FOR van_rec IN vans_cursor LOOP
--     json_data := van_rec.json_data;
--     UTL_FILE.PUT_LINE(output_file, json_data);
--   END LOOP;
--
-- UTL_FILE.FCLOSE(output_file);
-- return_out := 1;
-- END;
-- end;
-- DECLARE RET NUMBER(1);
-- BEGIN
--     FROM_JSON(RET);
-- end;
--
-- ---------FROM JSON-------------
-- CREATE OR REPLACE PROCEDURE FROM_JSON(RETURN_OUT OUT NUMBER)
-- AS BEGIN
--     INSERT INTO VANS("TYPE", CAPACITY, IS_FREE)
--      SELECT "type", capacity, is_free
-- FROM   JSON_TABLE(BFILENAME('UTL_DIR', 'VANS.JSON'), '$[*]'
--                   COLUMNS (
--                     "type" NUMBER PATH '$.type',
--                     capacity VARCHAR2(50) PATH '$.capacity',
--                     is_free NUMBER PATH '$.is_free'
--                   )
--                  );
--     RETURN_OUT := 1;
-- end;
--
-- CREATE TABLE VANS_EXTERNAL (
--   TYPE NVARCHAR2(15),
--   CAPACITY NUMBER,
--   IS_FREE NUMBER
-- )
-- ORGANIZATION EXTERNAL (
--   TYPE oracle_loader
--   DEFAULT DIRECTORY utl_dir
--   ACCESS PARAMETERS (
--     RECORDS DELIMITED BY NEWLINE
--     FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
--     MISSING FIELD VALUES ARE NULL
--   )
--   LOCATION ('VANS.JSON')
-- )
-- REJECT LIMIT UNLIMITED;
--
-- DROP TABLE VANS_EXTERNAL;
--
-- SELECT * FROM V$PARAMETER WHERE NAME LIKE 'parallel_execution_message_size';
--
-- SELECT *
-- FROM VANS_EXTERNAL where ROWNUM < 4;
--
-- -- SELECT * FROM MANAGER.TAKE_SCHEDULE;
-- --
-- SELECT * FROM ROLE_TAB_PRIVS where role like 'RLUSERCORE';
-- SELECT * FROM DBA_USERS WHERE USERNAME = 'USERS';
-- -- SELECT * FROM ROLE_TAB_PRIVS where role like 'RLMANAGERCORE';

SELECT * FROM MANAGER.VANS;
CALL INSERT_VANS('NEWVAN', 100, 1);
Select * from PASSENGERS;
UPDATE PASSENGERS set BENEFITS = '3' Where "ID" = 1;
CALL UPDATE_PASSENGERS(1, 'ABSOLUTELY NEW ERSON', 'MR2328434', 100);
SELECT * FROM TICKETS;
CALL INSERT_TICKETS(1, 21, 21, 10, 172, 212, ADD_MONTHS(SYSDATE, 4), 50000);
SELECT * FROM TRAINS;
CALL INSERT_TRAINS('TEST2', 1, '222,232,23', 7, 60);
SELECT * FROM STATIONS_ROUTES;
CALL INSERT_STATIONS_ROUTES(41, 21, 1);
SELECT * FROM STATIONS;
CALL INSERT_STATIONS('NEWSTATION1', 'МИНСК', 'МИНСКАЯ', 'БЕЛАРУСЬ');
SELECT * FROM SCHEDULE;
CALL INSERT_SCHEDULE(21, SYSDATE + 4, 41, 3);
SELECT * FROM ROUTES;
CALL INSERT_ROUTES(21, 48, 400, 200);
SELECT * FROM PAYMENTS;
CALL INSERT_PAYMENTS(21, SYSDATE, 'S');
CALL INSERT_TICKETS(1, 8, 2, 5, 161, 60, ADD_MONTHS(SYSDATE, 4), 50000);
CALL    INSERT_PAYMENTS(131, SYSDATE + 11, 'S');
SELECT * FROM PASSENGERS;
CALL INSERT_PASSENGERS('ABSOLUTELY NEW PERSON', 'MR2328434', 100);

SELECT * FROM MANAGER.TAKE_SCHEDULE;

SELECT * FROM MANAGER.TAKE_TICKET;
CALL INSERT_TAKE_TICKET(4, 21, 21, 23, 34, 76, SYSDATE + 10, 200000, SYSDATE + 2, 'S');

SELECT TYPE FROM MANAGER.VANS;

select * from DBA_OBJECTS where OBJECT_NAME = 'DBMS_JSON';
select * from TICKETS order by 'date' desc;

-- CALL UPDATE_TRAINS(17, 'TEST',1, VANS_COMPOSITION(221, 222,223,224), 4, 100);

CALL INSERT_ROUTES(1, 21, 200, 300);

SELECT * FROM TAKE_SCHEDULE;

CREATE OR REPLACE VIEW TAKE_SCHEDULE_USER AS
SELECT SCHEDULE."ID", ID_TRAIN, TRAINS.CATEGORY_OF_TRAIN,
       S2.STATION_NAME as DEPARTURE_POINT, S2.CITY as DEPARTURE_CITY, S.STATION_NAME as ARRIVAL_POINT, S.CITY as ARRIVAL_CITY,
       ROUTES.DISTANCE, ROUTES.DURATION,
       "DATE", FREQUENCY, TRAINS.IS_FOR_PASSENGERS
FROM SCHEDULE
         JOIN TRAINS ON SCHEDULE.ID_TRAIN = TRAINS.ID
         JOIN ROUTES ON SCHEDULE.ROUTE = ROUTES.ID
         JOIN STATIONS S on ROUTES.ARRIVAL_POINT = S.ID
         JOIN STATIONS S2 on ROUTES.DEPARTURE_POINT = S2.ID;

select *
from MANAGER.TAKE_SCHEDULE_USER;
-- SELECT * FROM MANAGER.TAKE_SCHEDULE_USER WHERE ARRIVAL_POINT = 'NewSt' AND DEPARTURE_POINT = 'NewSt' AND "DATE" <= TO_DATE('1:00', 'HH24:MI') AND ROWNUM > 0 AND ROWNUM <= 50 ORDER BY ID desc;

GRANT CREATE SESSION,
    CREATE TABLE,
    CREATE VIEW,
    CREATE PROCEDURE,
    CREATE TRIGGER TO RLMANAGERCORE;
--
GRANT ALL ON TICKETS TO RLMANAGERCORE;
--
GRANT ALL ON TRAINS TO RLMANAGERCORE;
--
GRANT ALL ON VANS TO RLMANAGERCORE;
--
GRANT ALL ON SCHEDULE TO RLMANAGERCORE;
--
GRANT ALL ON PASSENGERS TO RLMANAGERCORE;

GRANT ALL ON PAYMENTS TO RLMANAGERCORE;

GRANT ALL ON STATIONS_ROUTES TO RLMANAGERCORE;

GRANT ALL ON STATIONS TO RLMANAGERCORE;

GRANT ALL ON ROUTES TO RLMANAGERCORE;

------PERMISSIONS FOR USERS---------

GRANT CREATE SESSION TO RLUSERCORE;
--
GRANT
    SELECT,
    INSERT,
    UPDATE ON MANAGER.PASSENGERS TO RLUSERCORE;
--
GRANT
    SELECT,
    INSERT,
    UPDATE ON MANAGER.TICKETS TO RLUSERCORE;
--
GRANT
    SELECT ON MANAGER.SCHEDULE TO RLUSERCORE;

GRANT select ON MANAGER.VANS To RLUSERCORE;

GRANT SELECT ON MANAGER.TAKE_SCHEDULE TO RLUSERCORE;

GRANT SELECT ON MANAGER.TAKE_SCHEDULE_USER TO RLUSERCORE;

GRANT
    SELECT,
    INSERT ON MANAGER.PAYMENTS TO RLUSERCORE;

GRANT SELECT ON MANAGER.STATIONS TO RLUSERCORE;

GRANT SELECT ON MANAGER.STATIONS_ROUTES TO RLUSERCORE;

GRANT SELECT ON MANAGER.ROUTES TO RLUSERCORE;

GRANT SELECT ON MANAGER.TRAINS TO RLUSERCORE;

GRANT SELECT, INSERT, UPDATE ON MANAGER.TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.INSERT_TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.UPDATE_TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.DELETE_TAKE_TICKET TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.INSERT_PAYMENTS TO RLUSERCORE;

----???? БОТУ ПРОВЕРКИ ОПЛАТЫ ????
GRANT EXECUTE ON MANAGER.UPDATE_PAYMENTS TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.INSERT_PASSENGERS TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.UPDATE_PASSENGERS TO RLUSERCORE;

GRANT EXECUTE ON MANAGER.DELETE_PASSENGERS TO RLUSERCORE;