PGDMP     (                    z         
   Armazem_3D    13.3    14.1 _    $           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            %           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            &           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            '           1262    16394 
   Armazem_3D    DATABASE     p   CREATE DATABASE "Armazem_3D" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'English_United States.1252';
    DROP DATABASE "Armazem_3D";
                postgres    false            (           0    0 
   Armazem_3D    DATABASE PROPERTIES     K   ALTER DATABASE "Armazem_3D" SET search_path TO '$user', 'public', 'tiger';
                     postgres    false                        2615    20803    tiger    SCHEMA        CREATE SCHEMA tiger;
    DROP SCHEMA tiger;
                postgres    false                        2615    21081 
   tiger_data    SCHEMA        CREATE SCHEMA tiger_data;
    DROP SCHEMA tiger_data;
                postgres    false                        2615    20576    topology    SCHEMA        CREATE SCHEMA topology;
    DROP SCHEMA topology;
                postgres    false            )           0    0    SCHEMA topology    COMMENT     9   COMMENT ON SCHEMA topology IS 'PostGIS Topology schema';
                   postgres    false    15                        3079    20753    address_standardizer 	   EXTENSION     H   CREATE EXTENSION IF NOT EXISTS address_standardizer WITH SCHEMA public;
 %   DROP EXTENSION address_standardizer;
                   false            *           0    0    EXTENSION address_standardizer    COMMENT     �   COMMENT ON EXTENSION address_standardizer IS 'Used to parse an address into constituent elements. Generally used to support geocoding address normalization step.';
                        false    7                        3079    20760    address_standardizer_data_us 	   EXTENSION     P   CREATE EXTENSION IF NOT EXISTS address_standardizer_data_us WITH SCHEMA public;
 -   DROP EXTENSION address_standardizer_data_us;
                   false            +           0    0 &   EXTENSION address_standardizer_data_us    COMMENT     `   COMMENT ON EXTENSION address_standardizer_data_us IS 'Address Standardizer US dataset example';
                        false    8                        3079    20742    fuzzystrmatch 	   EXTENSION     A   CREATE EXTENSION IF NOT EXISTS fuzzystrmatch WITH SCHEMA public;
    DROP EXTENSION fuzzystrmatch;
                   false            ,           0    0    EXTENSION fuzzystrmatch    COMMENT     ]   COMMENT ON EXTENSION fuzzystrmatch IS 'determine similarities and distance between strings';
                        false    6                        3079    19009    postgis 	   EXTENSION     ;   CREATE EXTENSION IF NOT EXISTS postgis WITH SCHEMA public;
    DROP EXTENSION postgis;
                   false            -           0    0    EXTENSION postgis    COMMENT     g   COMMENT ON EXTENSION postgis IS 'PostGIS geometry, geography, and raster spatial types and functions';
                        false    2                        3079    20024    postgis_raster 	   EXTENSION     B   CREATE EXTENSION IF NOT EXISTS postgis_raster WITH SCHEMA public;
    DROP EXTENSION postgis_raster;
                   false    2            .           0    0    EXTENSION postgis_raster    COMMENT     M   COMMENT ON EXTENSION postgis_raster IS 'PostGIS raster types and functions';
                        false    3                        3079    20722    postgis_sfcgal 	   EXTENSION     B   CREATE EXTENSION IF NOT EXISTS postgis_sfcgal WITH SCHEMA public;
    DROP EXTENSION postgis_sfcgal;
                   false    2            /           0    0    EXTENSION postgis_sfcgal    COMMENT     C   COMMENT ON EXTENSION postgis_sfcgal IS 'PostGIS SFCGAL functions';
                        false    5            	            3079    20804    postgis_tiger_geocoder 	   EXTENSION     I   CREATE EXTENSION IF NOT EXISTS postgis_tiger_geocoder WITH SCHEMA tiger;
 '   DROP EXTENSION postgis_tiger_geocoder;
                   false    6    12    2            0           0    0     EXTENSION postgis_tiger_geocoder    COMMENT     ^   COMMENT ON EXTENSION postgis_tiger_geocoder IS 'PostGIS tiger geocoder and reverse geocoder';
                        false    9                        3079    20577    postgis_topology 	   EXTENSION     F   CREATE EXTENSION IF NOT EXISTS postgis_topology WITH SCHEMA topology;
 !   DROP EXTENSION postgis_topology;
                   false    15    2            1           0    0    EXTENSION postgis_topology    COMMENT     Y   COMMENT ON EXTENSION postgis_topology IS 'PostGIS topology spatial types and functions';
                        false    4            "           1259    21240    tbl_armazem    TABLE     �   CREATE TABLE public.tbl_armazem (
    arm_id integer NOT NULL,
    arm_nome character varying,
    arm_pol_id integer,
    arm_ativo boolean
);
    DROP TABLE public.tbl_armazem;
       public         heap    postgres    false            #           1259    21246    tbl_armazem_arm_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_armazem ALTER COLUMN arm_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_armazem_arm_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    290            $           1259    21248 
   tbl_camada    TABLE     �   CREATE TABLE public.tbl_camada (
    cam_id integer NOT NULL,
    cam_nome character varying,
    cam_cor character varying,
    cam_ativo boolean,
    cam_arm_id integer
);
    DROP TABLE public.tbl_camada;
       public         heap    postgres    false            %           1259    21254    tbl_camada_cam_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_camada ALTER COLUMN cam_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_camada_cam_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    292            &           1259    21256    tbl_estante    TABLE     �  CREATE TABLE public.tbl_estante (
    est_id integer NOT NULL,
    est_quantidade_prateleiras integer,
    est_prateleira_largura double precision,
    est_prateleira_altura double precision,
    est_prateleira_profundidade double precision,
    est_prateleira_peso_maximo double precision,
    est_pol_id integer,
    est_usu_id integer,
    est_ativo boolean,
    est_arm_id integer,
    est_ancoragem_lat double precision,
    est_ancoragem_lng double precision
);
    DROP TABLE public.tbl_estante;
       public         heap    postgres    false            '           1259    21259    tbl_estante_est_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_estante ALTER COLUMN est_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_estante_est_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    294            (           1259    21261    tbl_item_estoque    TABLE     U  CREATE TABLE public.tbl_item_estoque (
    ite_id integer NOT NULL,
    ite_usu_id integer,
    ite_data_hora timestamp without time zone,
    ite_ativo boolean,
    ite_tie_id integer,
    ite_pack_x double precision,
    ite_pack_y double precision,
    ite_pack_z double precision,
    ite_pra_id integer,
    ite_item_base_id integer
);
 $   DROP TABLE public.tbl_item_estoque;
       public         heap    postgres    false            )           1259    21264    tbl_item_estoque_ite_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_item_estoque ALTER COLUMN ite_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_item_estoque_ite_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    296            *           1259    21266    tbl_poligono    TABLE       CREATE TABLE public.tbl_poligono (
    pol_id integer NOT NULL,
    pol_cam_id integer,
    pol_geometria public.geometry GENERATED ALWAYS AS (public.st_geomfromgeojson(((pol_geojson)::json ->> 'geometry'::text))) STORED,
    pol_geojson character varying,
    pol_ativo boolean
);
     DROP TABLE public.tbl_poligono;
       public         heap    postgres    false    2    2    2    2    2    2    2    2    2    2    2    2    2    2    2    2    2            +           1259    21273    tbl_poligono_pol_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_poligono ALTER COLUMN pol_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_poligono_pol_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    298            ,           1259    21275    tbl_prateleira    TABLE     s   CREATE TABLE public.tbl_prateleira (
    pra_id integer NOT NULL,
    pra_est_id integer,
    pra_nivel integer
);
 "   DROP TABLE public.tbl_prateleira;
       public         heap    postgres    false            -           1259    21278    tbl_prateleira_pra_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_prateleira ALTER COLUMN pra_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_prateleira_pra_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    300            .           1259    21280    tbl_sistema_configuracao    TABLE     �   CREATE TABLE public.tbl_sistema_configuracao (
    sic_id integer NOT NULL,
    sic_nome character varying,
    sic_valor character varying,
    sic_ativo boolean
);
 ,   DROP TABLE public.tbl_sistema_configuracao;
       public         heap    postgres    false            /           1259    21286 #   tbl_sistema_configuracao_sic_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_sistema_configuracao ALTER COLUMN sic_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_sistema_configuracao_sic_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    302            0           1259    21288    tbl_tipo_item_estoque    TABLE     �  CREATE TABLE public.tbl_tipo_item_estoque (
    tie_id integer NOT NULL,
    tie_nome character varying,
    tie_descricao character varying,
    tie_largura double precision,
    tie_altura double precision,
    tie_profundidade double precision,
    tie_peso double precision,
    tie_peso_maximo_empilhamento double precision,
    tie_codigo_de_barras character varying,
    tie_usu_id integer,
    tie_data_hora timestamp without time zone,
    tie_ativo boolean
);
 )   DROP TABLE public.tbl_tipo_item_estoque;
       public         heap    postgres    false            1           1259    21294     tbl_tipo_item_estoque_tie_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_tipo_item_estoque ALTER COLUMN tie_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_tipo_item_estoque_tie_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    304            2           1259    21296    tbl_tipo_usuario    TABLE     }   CREATE TABLE public.tbl_tipo_usuario (
    tpu_id integer NOT NULL,
    tpu_nome character varying,
    tpu_ativo boolean
);
 $   DROP TABLE public.tbl_tipo_usuario;
       public         heap    postgres    false            3           1259    21302    tbl_tipo_usuario_tpu_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_tipo_usuario ALTER COLUMN tpu_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_tipo_usuario_tpu_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    306            4           1259    21304    tbl_usuario    TABLE     �   CREATE TABLE public.tbl_usuario (
    usu_id integer NOT NULL,
    usu_nome character varying,
    usu_senha character varying,
    usu_tpu_id integer,
    usu_ativo boolean
);
    DROP TABLE public.tbl_usuario;
       public         heap    postgres    false            5           1259    21310    tbl_usuario_usu_id_seq    SEQUENCE     �   ALTER TABLE public.tbl_usuario ALTER COLUMN usu_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_usuario_usu_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    308            �          0    19316    spatial_ref_sys 
   TABLE DATA           X   COPY public.spatial_ref_sys (srid, auth_name, auth_srid, srtext, proj4text) FROM stdin;
    public          postgres    false    212   Kp                 0    21240    tbl_armazem 
   TABLE DATA           N   COPY public.tbl_armazem (arm_id, arm_nome, arm_pol_id, arm_ativo) FROM stdin;
    public          postgres    false    290   hp                 0    21248 
   tbl_camada 
   TABLE DATA           V   COPY public.tbl_camada (cam_id, cam_nome, cam_cor, cam_ativo, cam_arm_id) FROM stdin;
    public          postgres    false    292   �p                 0    21256    tbl_estante 
   TABLE DATA             COPY public.tbl_estante (est_id, est_quantidade_prateleiras, est_prateleira_largura, est_prateleira_altura, est_prateleira_profundidade, est_prateleira_peso_maximo, est_pol_id, est_usu_id, est_ativo, est_arm_id, est_ancoragem_lat, est_ancoragem_lng) FROM stdin;
    public          postgres    false    294   q                 0    21261    tbl_item_estoque 
   TABLE DATA           �   COPY public.tbl_item_estoque (ite_id, ite_usu_id, ite_data_hora, ite_ativo, ite_tie_id, ite_pack_x, ite_pack_y, ite_pack_z, ite_pra_id, ite_item_base_id) FROM stdin;
    public          postgres    false    296   Lr                 0    21266    tbl_poligono 
   TABLE DATA           R   COPY public.tbl_poligono (pol_id, pol_cam_id, pol_geojson, pol_ativo) FROM stdin;
    public          postgres    false    298   vs                 0    21275    tbl_prateleira 
   TABLE DATA           G   COPY public.tbl_prateleira (pra_id, pra_est_id, pra_nivel) FROM stdin;
    public          postgres    false    300   `�                 0    21280    tbl_sistema_configuracao 
   TABLE DATA           Z   COPY public.tbl_sistema_configuracao (sic_id, sic_nome, sic_valor, sic_ativo) FROM stdin;
    public          postgres    false    302   Č                 0    21288    tbl_tipo_item_estoque 
   TABLE DATA           �   COPY public.tbl_tipo_item_estoque (tie_id, tie_nome, tie_descricao, tie_largura, tie_altura, tie_profundidade, tie_peso, tie_peso_maximo_empilhamento, tie_codigo_de_barras, tie_usu_id, tie_data_hora, tie_ativo) FROM stdin;
    public          postgres    false    304   (�                 0    21296    tbl_tipo_usuario 
   TABLE DATA           G   COPY public.tbl_tipo_usuario (tpu_id, tpu_nome, tpu_ativo) FROM stdin;
    public          postgres    false    306   ��                  0    21304    tbl_usuario 
   TABLE DATA           Y   COPY public.tbl_usuario (usu_id, usu_nome, usu_senha, usu_tpu_id, usu_ativo) FROM stdin;
    public          postgres    false    308   �       �          0    20777    us_gaz 
   TABLE DATA           J   COPY public.us_gaz (id, seq, word, stdword, token, is_custom) FROM stdin;
    public          postgres    false    236   3�       �          0    20763    us_lex 
   TABLE DATA           J   COPY public.us_lex (id, seq, word, stdword, token, is_custom) FROM stdin;
    public          postgres    false    234   P�       �          0    20791    us_rules 
   TABLE DATA           7   COPY public.us_rules (id, rule, is_custom) FROM stdin;
    public          postgres    false    238   m�       �          0    20810    geocode_settings 
   TABLE DATA           T   COPY tiger.geocode_settings (name, setting, unit, category, short_desc) FROM stdin;
    tiger          postgres    false    240   ��       �          0    21173    pagc_gaz 
   TABLE DATA           K   COPY tiger.pagc_gaz (id, seq, word, stdword, token, is_custom) FROM stdin;
    tiger          postgres    false    285   ��       �          0    21185    pagc_lex 
   TABLE DATA           K   COPY tiger.pagc_lex (id, seq, word, stdword, token, is_custom) FROM stdin;
    tiger          postgres    false    287   Ď       �          0    21197 
   pagc_rules 
   TABLE DATA           8   COPY tiger.pagc_rules (id, rule, is_custom) FROM stdin;
    tiger          postgres    false    289   �       �          0    20580    topology 
   TABLE DATA           G   COPY topology.topology (id, name, srid, "precision", hasz) FROM stdin;
    topology          postgres    false    227   ��       �          0    20593    layer 
   TABLE DATA           �   COPY topology.layer (topology_id, layer_id, schema_name, table_name, feature_column, feature_type, level, child_id) FROM stdin;
    topology          postgres    false    228   �       2           0    0    tbl_armazem_arm_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.tbl_armazem_arm_id_seq', 1, true);
          public          postgres    false    291            3           0    0    tbl_camada_cam_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.tbl_camada_cam_id_seq', 4, true);
          public          postgres    false    293            4           0    0    tbl_estante_est_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.tbl_estante_est_id_seq', 32, true);
          public          postgres    false    295            5           0    0    tbl_item_estoque_ite_id_seq    SEQUENCE SET     K   SELECT pg_catalog.setval('public.tbl_item_estoque_ite_id_seq', 390, true);
          public          postgres    false    297            6           0    0    tbl_poligono_pol_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.tbl_poligono_pol_id_seq', 101, true);
          public          postgres    false    299            7           0    0    tbl_prateleira_pra_id_seq    SEQUENCE SET     H   SELECT pg_catalog.setval('public.tbl_prateleira_pra_id_seq', 26, true);
          public          postgres    false    301            8           0    0 #   tbl_sistema_configuracao_sic_id_seq    SEQUENCE SET     Q   SELECT pg_catalog.setval('public.tbl_sistema_configuracao_sic_id_seq', 2, true);
          public          postgres    false    303            9           0    0     tbl_tipo_item_estoque_tie_id_seq    SEQUENCE SET     N   SELECT pg_catalog.setval('public.tbl_tipo_item_estoque_tie_id_seq', 4, true);
          public          postgres    false    305            :           0    0    tbl_tipo_usuario_tpu_id_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.tbl_tipo_usuario_tpu_id_seq', 1, false);
          public          postgres    false    307            ;           0    0    tbl_usuario_usu_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.tbl_usuario_usu_id_seq', 1, false);
          public          postgres    false    309            h           2606    21315    tbl_armazem tbl_armazem_pkey 
   CONSTRAINT     ^   ALTER TABLE ONLY public.tbl_armazem
    ADD CONSTRAINT tbl_armazem_pkey PRIMARY KEY (arm_id);
 F   ALTER TABLE ONLY public.tbl_armazem DROP CONSTRAINT tbl_armazem_pkey;
       public            postgres    false    290            j           2606    21317    tbl_camada tbl_camada_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.tbl_camada
    ADD CONSTRAINT tbl_camada_pkey PRIMARY KEY (cam_id);
 D   ALTER TABLE ONLY public.tbl_camada DROP CONSTRAINT tbl_camada_pkey;
       public            postgres    false    292            l           2606    21319    tbl_estante tbl_estante_pkey 
   CONSTRAINT     ^   ALTER TABLE ONLY public.tbl_estante
    ADD CONSTRAINT tbl_estante_pkey PRIMARY KEY (est_id);
 F   ALTER TABLE ONLY public.tbl_estante DROP CONSTRAINT tbl_estante_pkey;
       public            postgres    false    294            n           2606    21321 &   tbl_item_estoque tbl_item_estoque_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.tbl_item_estoque
    ADD CONSTRAINT tbl_item_estoque_pkey PRIMARY KEY (ite_id);
 P   ALTER TABLE ONLY public.tbl_item_estoque DROP CONSTRAINT tbl_item_estoque_pkey;
       public            postgres    false    296            p           2606    21323    tbl_poligono tbl_poligono_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.tbl_poligono
    ADD CONSTRAINT tbl_poligono_pkey PRIMARY KEY (pol_id);
 H   ALTER TABLE ONLY public.tbl_poligono DROP CONSTRAINT tbl_poligono_pkey;
       public            postgres    false    298            r           2606    21325 "   tbl_prateleira tbl_prateleira_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public.tbl_prateleira
    ADD CONSTRAINT tbl_prateleira_pkey PRIMARY KEY (pra_id);
 L   ALTER TABLE ONLY public.tbl_prateleira DROP CONSTRAINT tbl_prateleira_pkey;
       public            postgres    false    300            t           2606    21327 6   tbl_sistema_configuracao tbl_sistema_configuracao_pkey 
   CONSTRAINT     x   ALTER TABLE ONLY public.tbl_sistema_configuracao
    ADD CONSTRAINT tbl_sistema_configuracao_pkey PRIMARY KEY (sic_id);
 `   ALTER TABLE ONLY public.tbl_sistema_configuracao DROP CONSTRAINT tbl_sistema_configuracao_pkey;
       public            postgres    false    302            v           2606    21329 0   tbl_tipo_item_estoque tbl_tipo_item_estoque_pkey 
   CONSTRAINT     r   ALTER TABLE ONLY public.tbl_tipo_item_estoque
    ADD CONSTRAINT tbl_tipo_item_estoque_pkey PRIMARY KEY (tie_id);
 Z   ALTER TABLE ONLY public.tbl_tipo_item_estoque DROP CONSTRAINT tbl_tipo_item_estoque_pkey;
       public            postgres    false    304            x           2606    21331 &   tbl_tipo_usuario tbl_tipo_usuario_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.tbl_tipo_usuario
    ADD CONSTRAINT tbl_tipo_usuario_pkey PRIMARY KEY (tpu_id);
 P   ALTER TABLE ONLY public.tbl_tipo_usuario DROP CONSTRAINT tbl_tipo_usuario_pkey;
       public            postgres    false    306            z           2606    21333    tbl_usuario tbl_usuario_pkey 
   CONSTRAINT     ^   ALTER TABLE ONLY public.tbl_usuario
    ADD CONSTRAINT tbl_usuario_pkey PRIMARY KEY (usu_id);
 F   ALTER TABLE ONLY public.tbl_usuario DROP CONSTRAINT tbl_usuario_pkey;
       public            postgres    false    308            {           2606    21334 '   tbl_armazem tbl_armazem_arm_pol_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_armazem
    ADD CONSTRAINT tbl_armazem_arm_pol_id_fkey FOREIGN KEY (arm_pol_id) REFERENCES public.tbl_poligono(pol_id);
 Q   ALTER TABLE ONLY public.tbl_armazem DROP CONSTRAINT tbl_armazem_arm_pol_id_fkey;
       public          postgres    false    4976    298    290            |           2606    21339 '   tbl_estante tbl_estante_est_pol_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_estante
    ADD CONSTRAINT tbl_estante_est_pol_id_fkey FOREIGN KEY (est_pol_id) REFERENCES public.tbl_poligono(pol_id);
 Q   ALTER TABLE ONLY public.tbl_estante DROP CONSTRAINT tbl_estante_est_pol_id_fkey;
       public          postgres    false    4976    294    298            }           2606    21344 '   tbl_estante tbl_estante_est_usu_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_estante
    ADD CONSTRAINT tbl_estante_est_usu_id_fkey FOREIGN KEY (est_usu_id) REFERENCES public.tbl_usuario(usu_id);
 Q   ALTER TABLE ONLY public.tbl_estante DROP CONSTRAINT tbl_estante_est_usu_id_fkey;
       public          postgres    false    4986    308    294            ~           2606    21349 1   tbl_item_estoque tbl_item_estoque_ite_pra_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_item_estoque
    ADD CONSTRAINT tbl_item_estoque_ite_pra_id_fkey FOREIGN KEY (ite_pra_id) REFERENCES public.tbl_prateleira(pra_id) NOT VALID;
 [   ALTER TABLE ONLY public.tbl_item_estoque DROP CONSTRAINT tbl_item_estoque_ite_pra_id_fkey;
       public          postgres    false    4978    300    296                       2606    21354 1   tbl_item_estoque tbl_item_estoque_ite_tie_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_item_estoque
    ADD CONSTRAINT tbl_item_estoque_ite_tie_id_fkey FOREIGN KEY (ite_tie_id) REFERENCES public.tbl_tipo_item_estoque(tie_id);
 [   ALTER TABLE ONLY public.tbl_item_estoque DROP CONSTRAINT tbl_item_estoque_ite_tie_id_fkey;
       public          postgres    false    304    4982    296            �           2606    21359 1   tbl_item_estoque tbl_item_estoque_ite_usu_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_item_estoque
    ADD CONSTRAINT tbl_item_estoque_ite_usu_id_fkey FOREIGN KEY (ite_usu_id) REFERENCES public.tbl_usuario(usu_id);
 [   ALTER TABLE ONLY public.tbl_item_estoque DROP CONSTRAINT tbl_item_estoque_ite_usu_id_fkey;
       public          postgres    false    308    4986    296            �           2606    21364 )   tbl_poligono tbl_poligono_pol_cam_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_poligono
    ADD CONSTRAINT tbl_poligono_pol_cam_id_fkey FOREIGN KEY (pol_cam_id) REFERENCES public.tbl_camada(cam_id);
 S   ALTER TABLE ONLY public.tbl_poligono DROP CONSTRAINT tbl_poligono_pol_cam_id_fkey;
       public          postgres    false    4970    292    298            �           2606    21369 -   tbl_prateleira tbl_prateleira_pra_est_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_prateleira
    ADD CONSTRAINT tbl_prateleira_pra_est_id_fkey FOREIGN KEY (pra_est_id) REFERENCES public.tbl_estante(est_id);
 W   ALTER TABLE ONLY public.tbl_prateleira DROP CONSTRAINT tbl_prateleira_pra_est_id_fkey;
       public          postgres    false    4972    300    294            �           2606    21374 ;   tbl_tipo_item_estoque tbl_tipo_item_estoque_tie_usu_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_tipo_item_estoque
    ADD CONSTRAINT tbl_tipo_item_estoque_tie_usu_id_fkey FOREIGN KEY (tie_usu_id) REFERENCES public.tbl_usuario(usu_id);
 e   ALTER TABLE ONLY public.tbl_tipo_item_estoque DROP CONSTRAINT tbl_tipo_item_estoque_tie_usu_id_fkey;
       public          postgres    false    308    4986    304            �           2606    21379 '   tbl_usuario tbl_usuario_usu_tpu_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.tbl_usuario
    ADD CONSTRAINT tbl_usuario_usu_tpu_id_fkey FOREIGN KEY (usu_tpu_id) REFERENCES public.tbl_tipo_usuario(tpu_id);
 Q   ALTER TABLE ONLY public.tbl_usuario DROP CONSTRAINT tbl_usuario_usu_tpu_id_fkey;
       public          postgres    false    306    308    4984            �      x������ � �            x�3�t,�M�:�2W���Ȕ��+F��� U!�         \   x�3�t,�M�:�2�S���<%5���Ӑ˘�)1'���|N�ԤTSc���kqIb^Ij1Hqj��%X؄�9��(5%�,�dh�������� ~��         9  x�]R[�!�ƻ��p�� �{��-�$;���H�4��{��n@���'� ��z6-�G�;��.II�8!]�-��ĬlX�����:}�Y���|�ύ�W=q/�gX�b�% �] �]�̂~�� �n"�Bl�`��JL>�e�ưKx�,3M��?��>��^��z��.����hn]Z)[NɹJ��qڌl씵#J`��-m"ם%-��@��":��X���ڻS�ۈ�W��+�F���5�Q$��R�T�S�H�;{�jR�^V�߼ݿX��n����[��c�iӯܤ���Z[=��{tj�zk����           x�m�ۑ�0�oSE�FoK�
R��?�XBV�_��J���x>#��Nv�\W��?���:������U	rJ�l�}h��r�d'��8��'�=��bz�>H7~���K�Se���������{�f�v��.�Q[.D�4�j�3���� �ι�y1�C�k^2�'����P�?ͿS�2Jv�(���.��s�Y��G��0�󣂬���v���3�w	����K��qm�a�v�Q��/���p7y)������B�z��',��?��3            x��]͎e�m^O?������Djv�� � @��,��` ϴ��,&��&�<�_,��H��Tu��co���<��I��*�o��?����>����?x���>}x����>��ç�?~���o����?���>���_��/�$���������3�����>�����ϟ���Oo� m�?��O�/���"��~����~����7��ͻ�ʅ�S��N�6����2���\���G��������o~�&o�.�K���KO�2��R���z�i��|���J��s�&�8��oҩ�r�2[�����2��o�|�����_��ڒ���{�m�Q�r�7��xٞ3XN[,��d�.���� \��+p����a�ط%��	|A]��S���oV~��{���x5�2�}5��	��*L>_��o�}��x[e�t�V�����������-�K��e��p�͵�:̭�~�l5O�붞��B����������||ň�Z?M��.�-��f .��������m{ڶ��P�Ez�$*VDп���s�?i��RB_\Q����s��w�����;ф���9M#<��m���$ݪ��V�g�7O�8x�����+5��q��9e[�r���K� ���~���6e��M��=Qn����O�>|��ӳ��_���|����?܋*V�\:3pk�R�a�n��1�#�	�C��o����uc�_�����C����,nG	��;��:������.����4[����� ��a���<��q���������~�ӫ��/������Ί��5]!�U�?m%�m�n�俞o�^�r�p:�Yt�]ׅz;���C��Q�Mw�"v��4>��(�^�N^�|i���N��ƭ��x ��|������Q��`�&��x ��������(�sU{1Ap_<]l�f��u�EA�����G�d��~�4Du�;�a�It��2�T"�8�%$#O�F�m!��è`�@��Vh!XHO=U^HI�to$�* 4�V���/D��p� ���H����� �`yT0O���2 �ǂ`���2j�-��`��}��ṱ����\i d{XK�s  ӭ1�Np �S@���4�ȹ��؏)�p@ؘ�	�c��T�h���;3��<�06@��9_ęo��@�%�DzI="|�e�@��kn�� �T~&�d<#�ƳLBa��b��8	� eB�y��C5J���\c�|� n%c3�����ٳ!��Js�)"������є䫣�AV�1]dF��Ă��`K��*�hNS�?��D�4"r	�-�6כ�SQN�e���؆@Vz��!X�����~&��dБ��x��.��z��l�nSRޑZ؟�:��'-M����<�����U��83�h�l��g�ÆtY{���!_�>!�~X�������p�l���c����������Û�^�����@�h*"Qak�r�G�> M�B�U��L��[�D@q�fK����^���ç.a���g�W��B�c��j
��"���u�}L�	����:���
	OSH⯌M��m�>열Z��c�7����G�Ɯ��k˶Ѣ�:C����4�����2�}�h�}����3����[P}?�BgGZd�d���\���˺�7D��>`cmZ�󛾟7�g���̪��hfa<�e�'��L� �j����r�����WL� �-�z%�҇h�i�I�􇭣�/��:ڄ��󌼉�µ��˒-٥��i%Y��:b:ƚ}�����������w��.�X�!+�{�y���.�=�������qQ���y�E���7����H�t`�X�,�����?5��L�r���x잣Q��T@��6�e_q�����;S��%�f��ܡ��4�E(:�˴�X(/�B��������jQ_Ѡ�"�eN�Lb�q!G8H�\{���EM���3����a��rQ�s��h�Oa�f�hE��)�;Z�u 5gK]�:��,��R� ��-��j��4�����Ԅe2-}��:��4�*/�����@��|k!Pq�G�!�G%=C-���4��UBhy��K���dXt�Ю��b��n�M�4��.#�#&y���> $N�!�t1�N�!0C�5�����҃sz���R[�eш6p�^���Դ�l��P�!���H�5%�L���FF��"2� Mg#&�X�!��{��xo���A��8/ZFAz�6��X�2ds�B)�~��F{���M�,�ll/�NAf ��Ü��g�Ϝy>LTT��Z-��Sd�;���2Op!,�ƢkJH�s�vڴ�FV�T�I�� �<�2d,���*25�(g� ����:utuq}'�VW�I�«�H4�|�>y*\b��b.Π��� v P��e���b� �L`3ΤI�R�4�<S0Y�#a��GFIJ`�E��<��H���*"�"
�Tf:Et*��uK��@�L�+	�<���Zf��Ϣg>GTe-��Z�Lv��`�k4�v koEX&��C�f��a��n�Q���[����!���hA�.ffO&T���R'KY���G �B��H^KZTjm�N)�4M����&Wݲ6K5���9���,�#[�9�����fᠻ� �����G���j*R�C��Q6G���9����j�1%g2�k��Tg:�䯑m%wgZ4��M}� E�{t�筤��J�|�׈]$�,�F�QC⯁.��;���^� O��"ZfZ�_��4�e�Հ{NO}7���N�r�����z�@PA�i���_�*�� t��I"��aˡP�!�N�m���?M���,�ב=ߘJ����:����e�~�W>����Y�?̦�<E�w������y��/�+c�_Q(����6]I��}8����/ ϧU�kD�F�٤�a��'���Sw����_�O��}%�<�`�1E�^h{H|�����?�������[����9����<4�D��|ژ}�L�b;ڮ���ٶG���|?� �%�U�fI���LO�ѮƗ���G��h3�Y����&�~�NLQ�eVg6֧/�}��������N�qµ�!D_���K���&4C����#�m����]`�� ��h[+�9r��:�-�5������p���g����W*�W��Go�}_3�|�~�d[72M�ia��It�0iL�����^:e�&mdwR�m�r�ā��I�7�;�O��i�vI�1=�NG0�w�_�Ѕ��<�O3u���<��a�����K�G�ʥ�QN⥺��\�"�U��#j]+bѐ�+*���Uj˺�"�Z3�bߊdG k��m\�I[��0d� ���PЫInC2��u!Q9lHꋳ�26ZJ+p8r&V�Rz�+<��JGxE�G��z�PY=�%���ͭ��A*���L$^��a���B�Z�T) !p&��f�)��=C�d��:I�^��J��Wq�Imz�ޝv���9�}ؐ�������W-���*�X9E�R�D��S�W�dh�Z��.WtJ+JA^	���G�W�'�\����)$9�K�jPpc�"�n�����)�@��/�H��n�5��01�L�B�Sm�t�.����g0>C�9�֚Eɡ��#�{,*���9H:��W%�n���:B�i����f!ږ���zfs5�/���{d)�k��ê��4^�x,A-�;�W�5 �����)�3�F�Q�b���J(���F||!�+����rv�LBM�C8%Uّ�}M���«�B�6�B ��P홺���qA�.s�/���!ͭV�ݳ�Sg��*@�Y�H���=<�P�	rq�uU����Gp��D�1D֊��dQ�3�&'^��8K_�"�s0xѓe�y�M�#�%3>2���)�Z�Z�K4���%��L�RI���n���Hg�e��zd`w(�w���)]:s=m,��`�U�<#��Z�Ȩ~5WR{��u�kAP�>�Q	Y}���b=1�8����453Q(<��
2��`���W1z�VB��˚4Ц����5�W�����TCm� �  D5������|� ������2i*�&_�c���\{3����%�?L���B[H���h9�8�`���uI� /-��+Y�[��F�:�Fc?<\�_y��E� ܕ/_��B_����(�f)�f9r�{(pw`˗UnWH�2��fE���9O��������M��1��W���X5�~i=���.Zk��!\��P����}��%��bH|Ɍtߞc��v=��4��G���j����V�;����ކ�p�N-Ã��س߷Gc��e�2v���9O�v�@�߷�kxǤ�u�[
b·�'|�r�bC�n���mw:���O[ڍ��㠳���Z]i)���@�U@a!|�X�a��ϸ�q�蕺I��H�GL��~���~,��Ke�%��H���#����a���a��GY$㈌MX�B�웞z��y���g�Vۿ ��A��B�	[�ة���?���>�H�����^�,þ
og�����nY��V�|m�̡��A�g`��ztvP��yVۂ?�����Ȭ����5���^t�Gg����:6����>Ѿ������n�@2����J�8[� �x�d4P�'�l�n��55���w��rZ��1����Q8��d����3�[�@?���6|�k�,�!bU40o;Jz֞1��>YԀ_@�ggB���/
�HiJ��	��Hs$ �2b�=�}�����5�H�VG��b�4�7k�\)�`�S*��"�d�'Y�)���k�/G
���uL9b�#�EP���P�m-��S��$�H���Nپ�i����5E�2��8E؏���F��?M7 Y�E�#үH[G����ݯo���3'ʑ�ئ�t�?����Ϛr�ENfZ��_�-.�B�2�z�)�\q�&�e��ݺ�V�xUZMv�HVw�W���Ce8!jV[ k�42a'��pρt�M42E�Fn�"i��a�.@#��n�j�C�(#F���&�^#M�;�jK-д��Ak�=n��١�Z�,�xG�4��t�p��*��/i�AD�EO2�i*�9.`?�6�fg�dU���3c@E�_$�6��*�4�E#|�n�W�%�2�H��@C�*;A�--� L�8%?�3̳C��R�%�������@Mw�����d�li{�	�=$���CHш[���B� �)�h�"'��,���2C��=a��X0K)�+�CF��]tO�$�,e�R��i�~Q=��Hnf(����'ý՚#���T��Y�-�L�I�Y��[�=����̢�ҳ�2�h�T扌�A;����D���
r�H��B��o��-�B�$k!^�)�1M�)�����2�1Ӹ�Dzoч��J'ą�����J�A�aĲۍ��w����M|8n���e1I
�E��v��T;{����H�lx�����1-���'ug��4T#�]���H�y�'��HD#L��h�_�&�O9ҀU���A�4X,��4��i³zq�E;�(�(�4��YR=�P�4ҵ=EN�V�H�)e�W�4�/.EQa�l��Q 2e������>�c�=Ҡh�7�H�w��U�i�o.;�%i~bQ/��4duB��Q$Y�VcQ�ߒ���do�z�AF�&^�M�g/�T���_����cptv��2������U=���h�~|�Ϗ�ŭ8�U�Bb	�ow��u�SIɜpQ.zX��~l�Ύm?{ϝ��3��؅� �]kN	��hDً6Ε�)��@:;��z�I�ł!�o|!�D�q4:?	� ��4js��!w���pV(=�K�ݓ�}d�f�/�N�?�B�\��';sp[�آZkn�48	�L)�L����˝���}>;���j5c��X�"����o7�T�u��T�B������h�&�묲0@{�-B���(�5	�/��x��NZ��=�q�Xz!S��r>�D��=��쑽�KM}��^_: Op���k��Oe����)Ⱦg�g{�5���yBx$�u���FWk��/=��^=��7����h��E/���Gȵ����ƈ�B��c������m{^�+���C�o-��~y+a�er���}�Y�˗q*f�e��N����*ğfG����~�����*½��r��"�9�� �W\���,]֣�W'i�RD;��[vે۝e���8h��gTEI�%�E�Mmi���7q��q�C�R,i}X��������[F�[Ft��K�,m��YM�u�"2��.$<�yd=m��^޼y��)��         T   x����@�P��3^/�_��k���"l���` �E��"�41w��K5
�*��aO���?�מ����`a:�4߃���}         T   x�3�,K-*N��4�3�,�2�LJ�IN��M�K�/JLO���I,��K�V2��t��L�Mu���@�������Y-Ps� ۧB         �   x�U�=n�0Fg���񣨿-�=ti�x�V�@� �o�!��*!nӐ A�����4� �W�k�I��Ip���;��-BH)ѷ���:3��Y��"�S~���\4	��Th5x:���o��q��J��bP�⸞s礠�x�s�Յij.�8|L��mü|]Ϗ��viD�m�7p��Y� bA��.
���c~ �=`            x������ � �             x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �     