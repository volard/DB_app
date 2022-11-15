CREATE TABLE "HOSPITALS" (
	"id_hospital" serial NOT NULL,
	"Surename_main_doctor" character varying(30) NOT NULL,
	"Name_main_doctor" character varying(30) NOT NULL,
	"Middlename_main_doctor" character varying(30) NOT NULL,
	"INN" bigint NOT NULL UNIQUE,
	"OGRN" bigint NOT NULL UNIQUE,
	CONSTRAINT "HOSPITALS_pk" PRIMARY KEY ("id_hospital")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "PHARMACIES" (
	"id_pharmacy" serial NOT NULL,
	"Name" character varying(300) NOT NULL UNIQUE,
	"INN" bigint NOT NULL UNIQUE,
	"OGRN" bigint NOT NULL UNIQUE,
	CONSTRAINT "PHARMACIES_pk" PRIMARY KEY ("id_pharmacy")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "ORDERS" (
	"id_order" serial NOT NULL,
	"id_hospital" serial NOT NULL,
	"id_pharmacy" serial NOT NULL,
	CONSTRAINT "ORDERS_pk" PRIMARY KEY ("id_order")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "PRODUCTS" (
	"id_product" serial NOT NULL,
	"id_medicine" serial NOT NULL,
	"id_pharmacy" serial NOT NULL,
	"Price" money NOT NULL,
	"Amount" integer NOT NULL,
	CONSTRAINT "PRODUCTS_pk" PRIMARY KEY ("id_product")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "MEDICINES" (
	"id_medicine" serial NOT NULL,
	"Name" character varying(300) NOT NULL,
	"Type" character varying(100) NOT NULL,
	CONSTRAINT "MEDICINES_pk" PRIMARY KEY ("id_medicine")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "ADDRESSES" (
	"id_address" serial NOT NULL,
	"City" character varying(100) NOT NULL,
	"Street" character varying(100) NOT NULL,
	"Building" character varying(100) NOT NULL,
	CONSTRAINT "ADDRESSES_pk" PRIMARY KEY ("id_address")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "HOSPITALS_LOCATIONS" (
	"id_hospital" serial NOT NULL,
	"id_address" serial NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "PHARMACIES_LOCATIONS" (
	"id_pharmacy" serial NOT NULL,
	"id_address" serial NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "order_item" (
	"id_order_item" serial NOT NULL,
	"id_order" serial NOT NULL,
	"id_product" serial NOT NULL,
	"Amount" integer NOT NULL,
	CONSTRAINT "order_item_pk" PRIMARY KEY ("id_order_item")
) WITH (
  OIDS=FALSE
);





ALTER TABLE "ORDERS" ADD CONSTRAINT "ORDERS_fk0" FOREIGN KEY ("id_hospital") REFERENCES "HOSPITALS"("id_hospital");
ALTER TABLE "ORDERS" ADD CONSTRAINT "ORDERS_fk1" FOREIGN KEY ("id_pharmacy") REFERENCES "PHARMACIES"("id_pharmacy");

ALTER TABLE "PRODUCTS" ADD CONSTRAINT "PRODUCTS_fk0" FOREIGN KEY ("id_medicine") REFERENCES "MEDICINES"("id_medicine");
ALTER TABLE "PRODUCTS" ADD CONSTRAINT "PRODUCTS_fk1" FOREIGN KEY ("id_pharmacy") REFERENCES "PHARMACIES"("id_pharmacy");



ALTER TABLE "HOSPITALS_LOCATIONS" ADD CONSTRAINT "HOSPITALS_LOCATIONS_fk0" FOREIGN KEY ("id_hospital") REFERENCES "HOSPITALS"("id_hospital");
ALTER TABLE "HOSPITALS_LOCATIONS" ADD CONSTRAINT "HOSPITALS_LOCATIONS_fk1" FOREIGN KEY ("id_address") REFERENCES "ADDRESSES"("id_address");

ALTER TABLE "PHARMACIES_LOCATIONS" ADD CONSTRAINT "PHARMACIES_LOCATIONS_fk0" FOREIGN KEY ("id_pharmacy") REFERENCES "PHARMACIES"("id_pharmacy");
ALTER TABLE "PHARMACIES_LOCATIONS" ADD CONSTRAINT "PHARMACIES_LOCATIONS_fk1" FOREIGN KEY ("id_address") REFERENCES "ADDRESSES"("id_address");

ALTER TABLE "order_item" ADD CONSTRAINT "order_item_fk0" FOREIGN KEY ("id_order") REFERENCES "ORDERS"("id_order");
ALTER TABLE "order_item" ADD CONSTRAINT "order_item_fk1" FOREIGN KEY ("id_product") REFERENCES "PRODUCTS"("id_product");









