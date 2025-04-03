PGDMP  #                    }           ontu_phd    17.2    17.2 K    T           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            U           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            V           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            W           1262    18003    ontu_phd    DATABASE     |   CREATE DATABASE ontu_phd WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE ontu_phd;
                     postgres    false            �            1259    18004    applydocuments    TABLE     �   CREATE TABLE public.applydocuments (
    id integer NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    requirements jsonb NOT NULL,
    originalsrequired jsonb NOT NULL
);
 "   DROP TABLE public.applydocuments;
       public         heap r       postgres    false            �            1259    18009    applydocuments_id_seq    SEQUENCE     �   CREATE SEQUENCE public.applydocuments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.applydocuments_id_seq;
       public               postgres    false    217            X           0    0    applydocuments_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.applydocuments_id_seq OWNED BY public.applydocuments.id;
          public               postgres    false    218            �            1259    26290    defense    TABLE     }  CREATE TABLE public.defense (
    id integer NOT NULL,
    program_id integer NOT NULL,
    name_surname text NOT NULL,
    defense_name text,
    science_teachers text,
    date_of_defense timestamp without time zone NOT NULL,
    address text,
    description text,
    members jsonb,
    files jsonb,
    date_of_publication timestamp without time zone,
    placeholder text
);
    DROP TABLE public.defense;
       public         heap r       postgres    false            �            1259    26289    defense_id_seq    SEQUENCE     �   CREATE SEQUENCE public.defense_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.defense_id_seq;
       public               postgres    false    236            Y           0    0    defense_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.defense_id_seq OWNED BY public.defense.id;
          public               postgres    false    235            �            1259    18010 	   documents    TABLE     �   CREATE TABLE public.documents (
    id integer NOT NULL,
    programid integer,
    name text NOT NULL,
    type text NOT NULL,
    link text NOT NULL
);
    DROP TABLE public.documents;
       public         heap r       postgres    false            �            1259    18015    documents_id_seq    SEQUENCE     �   CREATE SEQUENCE public.documents_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.documents_id_seq;
       public               postgres    false    219            Z           0    0    documents_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.documents_id_seq OWNED BY public.documents.id;
          public               postgres    false    220            �            1259    18102 	   employees    TABLE     �   CREATE TABLE public.employees (
    id integer NOT NULL,
    name text NOT NULL,
    "position" text NOT NULL,
    photo text NOT NULL
);
    DROP TABLE public.employees;
       public         heap r       postgres    false            �            1259    18101    employees_id_seq    SEQUENCE     �   CREATE SEQUENCE public.employees_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.employees_id_seq;
       public               postgres    false    226            [           0    0    employees_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.employees_id_seq OWNED BY public.employees.id;
          public               postgres    false    225            �            1259    18112    field_of_study    TABLE     h   CREATE TABLE public.field_of_study (
    code character varying(50) NOT NULL,
    name text NOT NULL
);
 "   DROP TABLE public.field_of_study;
       public         heap r       postgres    false            �            1259    18132    job    TABLE     ~   CREATE TABLE public.job (
    id integer NOT NULL,
    code text NOT NULL,
    title text NOT NULL,
    program_id integer
);
    DROP TABLE public.job;
       public         heap r       postgres    false            �            1259    18131 
   job_id_seq    SEQUENCE     �   CREATE SEQUENCE public.job_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 !   DROP SEQUENCE public.job_id_seq;
       public               postgres    false    230            \           0    0 
   job_id_seq    SEQUENCE OWNED BY     9   ALTER SEQUENCE public.job_id_seq OWNED BY public.job.id;
          public               postgres    false    229            �            1259    18020    news    TABLE       CREATE TABLE public.news (
    id integer NOT NULL,
    title text NOT NULL,
    summary text NOT NULL,
    maintag text NOT NULL,
    othertags jsonb NOT NULL,
    date date NOT NULL,
    thumbnail text NOT NULL,
    photos jsonb NOT NULL,
    body text NOT NULL
);
    DROP TABLE public.news;
       public         heap r       postgres    false            �            1259    18025    news_id_seq    SEQUENCE     �   CREATE SEQUENCE public.news_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public.news_id_seq;
       public               postgres    false    221            ]           0    0    news_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.news_id_seq OWNED BY public.news.id;
          public               postgres    false    222            �            1259    18181    program    TABLE     t  CREATE TABLE public.program (
    id integer NOT NULL,
    degree character varying(100) NOT NULL,
    name text NOT NULL,
    name_code text,
    field_of_study jsonb,
    speciality jsonb,
    form jsonb DEFAULT '[]'::jsonb,
    purpose text,
    years integer,
    credits integer,
    program_characteristics jsonb,
    program_competence jsonb,
    results jsonb DEFAULT '[]'::jsonb,
    link_faculty text,
    link_file text,
    accredited boolean DEFAULT false,
    directions jsonb,
    objects text,
    CONSTRAINT program_credits_check CHECK ((credits > 0)),
    CONSTRAINT program_years_check CHECK ((years > 0))
);
    DROP TABLE public.program;
       public         heap r       postgres    false            �            1259    18180    program_id_seq    SEQUENCE     �   CREATE SEQUENCE public.program_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.program_id_seq;
       public               postgres    false    232            ^           0    0    program_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.program_id_seq OWNED BY public.program.id;
          public               postgres    false    231            �            1259    18205    programcomponents    TABLE     �   CREATE TABLE public.programcomponents (
    id integer NOT NULL,
    program_id integer,
    componenttype text NOT NULL,
    componentname text NOT NULL,
    componentcredits integer,
    componenthours integer,
    controlform jsonb
);
 %   DROP TABLE public.programcomponents;
       public         heap r       postgres    false            �            1259    18204    programcomponents_id_seq    SEQUENCE     �   CREATE SEQUENCE public.programcomponents_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public.programcomponents_id_seq;
       public               postgres    false    234            _           0    0    programcomponents_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.programcomponents_id_seq OWNED BY public.programcomponents.id;
          public               postgres    false    233            �            1259    18041    roadmaps    TABLE     �   CREATE TABLE public.roadmaps (
    id integer NOT NULL,
    type text NOT NULL,
    datastart date NOT NULL,
    dataend date,
    additionaltime text,
    description text NOT NULL
);
    DROP TABLE public.roadmaps;
       public         heap r       postgres    false            �            1259    18046    roadmaps_id_seq    SEQUENCE     �   CREATE SEQUENCE public.roadmaps_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.roadmaps_id_seq;
       public               postgres    false    223            `           0    0    roadmaps_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.roadmaps_id_seq OWNED BY public.roadmaps.id;
          public               postgres    false    224            �            1259    18119 
   speciality    TABLE     �   CREATE TABLE public.speciality (
    code character varying(50) NOT NULL,
    name text NOT NULL,
    field_code character varying(50) NOT NULL
);
    DROP TABLE public.speciality;
       public         heap r       postgres    false            �           2604    18047    applydocuments id    DEFAULT     v   ALTER TABLE ONLY public.applydocuments ALTER COLUMN id SET DEFAULT nextval('public.applydocuments_id_seq'::regclass);
 @   ALTER TABLE public.applydocuments ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    218    217            �           2604    26293 
   defense id    DEFAULT     h   ALTER TABLE ONLY public.defense ALTER COLUMN id SET DEFAULT nextval('public.defense_id_seq'::regclass);
 9   ALTER TABLE public.defense ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    235    236    236            �           2604    18048    documents id    DEFAULT     l   ALTER TABLE ONLY public.documents ALTER COLUMN id SET DEFAULT nextval('public.documents_id_seq'::regclass);
 ;   ALTER TABLE public.documents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219            �           2604    18105    employees id    DEFAULT     l   ALTER TABLE ONLY public.employees ALTER COLUMN id SET DEFAULT nextval('public.employees_id_seq'::regclass);
 ;   ALTER TABLE public.employees ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    226    225    226            �           2604    18135    job id    DEFAULT     `   ALTER TABLE ONLY public.job ALTER COLUMN id SET DEFAULT nextval('public.job_id_seq'::regclass);
 5   ALTER TABLE public.job ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    230    229    230            �           2604    18050    news id    DEFAULT     b   ALTER TABLE ONLY public.news ALTER COLUMN id SET DEFAULT nextval('public.news_id_seq'::regclass);
 6   ALTER TABLE public.news ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    222    221            �           2604    18184 
   program id    DEFAULT     h   ALTER TABLE ONLY public.program ALTER COLUMN id SET DEFAULT nextval('public.program_id_seq'::regclass);
 9   ALTER TABLE public.program ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    231    232    232            �           2604    18208    programcomponents id    DEFAULT     |   ALTER TABLE ONLY public.programcomponents ALTER COLUMN id SET DEFAULT nextval('public.programcomponents_id_seq'::regclass);
 C   ALTER TABLE public.programcomponents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    234    233    234            �           2604    18053    roadmaps id    DEFAULT     j   ALTER TABLE ONLY public.roadmaps ALTER COLUMN id SET DEFAULT nextval('public.roadmaps_id_seq'::regclass);
 :   ALTER TABLE public.roadmaps ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    224    223            >          0    18004    applydocuments 
   TABLE DATA           `   COPY public.applydocuments (id, name, description, requirements, originalsrequired) FROM stdin;
    public               postgres    false    217   �V       Q          0    26290    defense 
   TABLE DATA           �   COPY public.defense (id, program_id, name_surname, defense_name, science_teachers, date_of_defense, address, description, members, files, date_of_publication, placeholder) FROM stdin;
    public               postgres    false    236   �b       @          0    18010 	   documents 
   TABLE DATA           D   COPY public.documents (id, programid, name, type, link) FROM stdin;
    public               postgres    false    219   �e       G          0    18102 	   employees 
   TABLE DATA           @   COPY public.employees (id, name, "position", photo) FROM stdin;
    public               postgres    false    226   Ci       H          0    18112    field_of_study 
   TABLE DATA           4   COPY public.field_of_study (code, name) FROM stdin;
    public               postgres    false    227   j       K          0    18132    job 
   TABLE DATA           :   COPY public.job (id, code, title, program_id) FROM stdin;
    public               postgres    false    230   �j       B          0    18020    news 
   TABLE DATA           e   COPY public.news (id, title, summary, maintag, othertags, date, thumbnail, photos, body) FROM stdin;
    public               postgres    false    221   l       M          0    18181    program 
   TABLE DATA           �   COPY public.program (id, degree, name, name_code, field_of_study, speciality, form, purpose, years, credits, program_characteristics, program_competence, results, link_faculty, link_file, accredited, directions, objects) FROM stdin;
    public               postgres    false    232   �p       O          0    18205    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, program_id, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    234   �{       D          0    18041    roadmaps 
   TABLE DATA           ]   COPY public.roadmaps (id, type, datastart, dataend, additionaltime, description) FROM stdin;
    public               postgres    false    223   �}       I          0    18119 
   speciality 
   TABLE DATA           <   COPY public.speciality (code, name, field_code) FROM stdin;
    public               postgres    false    228   р       a           0    0    applydocuments_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.applydocuments_id_seq', 1, true);
          public               postgres    false    218            b           0    0    defense_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.defense_id_seq', 1, false);
          public               postgres    false    235            c           0    0    documents_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.documents_id_seq', 11, true);
          public               postgres    false    220            d           0    0    employees_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.employees_id_seq', 3, true);
          public               postgres    false    225            e           0    0 
   job_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.job_id_seq', 10, true);
          public               postgres    false    229            f           0    0    news_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.news_id_seq', 8, true);
          public               postgres    false    222            g           0    0    program_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.program_id_seq', 1, true);
          public               postgres    false    231            h           0    0    programcomponents_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.programcomponents_id_seq', 11, true);
          public               postgres    false    233            i           0    0    roadmaps_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.roadmaps_id_seq', 13, true);
          public               postgres    false    224            �           2606    18060 "   applydocuments applydocuments_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.applydocuments
    ADD CONSTRAINT applydocuments_pkey PRIMARY KEY (id);
 L   ALTER TABLE ONLY public.applydocuments DROP CONSTRAINT applydocuments_pkey;
       public                 postgres    false    217            �           2606    26297    defense defense_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.defense
    ADD CONSTRAINT defense_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.defense DROP CONSTRAINT defense_pkey;
       public                 postgres    false    236            �           2606    18062    documents documents_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.documents
    ADD CONSTRAINT documents_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.documents DROP CONSTRAINT documents_pkey;
       public                 postgres    false    219            �           2606    18109    employees employees_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.employees DROP CONSTRAINT employees_pkey;
       public                 postgres    false    226            �           2606    18118 "   field_of_study field_of_study_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.field_of_study
    ADD CONSTRAINT field_of_study_pkey PRIMARY KEY (code);
 L   ALTER TABLE ONLY public.field_of_study DROP CONSTRAINT field_of_study_pkey;
       public                 postgres    false    227            �           2606    18139    job job_pkey 
   CONSTRAINT     J   ALTER TABLE ONLY public.job
    ADD CONSTRAINT job_pkey PRIMARY KEY (id);
 6   ALTER TABLE ONLY public.job DROP CONSTRAINT job_pkey;
       public                 postgres    false    230            �           2606    18066    news news_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.news
    ADD CONSTRAINT news_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.news DROP CONSTRAINT news_pkey;
       public                 postgres    false    221            �           2606    18194    program program_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.program
    ADD CONSTRAINT program_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.program DROP CONSTRAINT program_pkey;
       public                 postgres    false    232            �           2606    18212 (   programcomponents programcomponents_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_pkey PRIMARY KEY (id);
 R   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_pkey;
       public                 postgres    false    234            �           2606    18074    roadmaps roadmaps_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.roadmaps
    ADD CONSTRAINT roadmaps_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.roadmaps DROP CONSTRAINT roadmaps_pkey;
       public                 postgres    false    223            �           2606    18125    speciality speciality_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.speciality
    ADD CONSTRAINT speciality_pkey PRIMARY KEY (code);
 D   ALTER TABLE ONLY public.speciality DROP CONSTRAINT speciality_pkey;
       public                 postgres    false    228            �           2606    18213 2   programcomponents programcomponents_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_programid_fkey FOREIGN KEY (program_id) REFERENCES public.program(id);
 \   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_programid_fkey;
       public               postgres    false    4774    234    232            �           2606    18126 %   speciality speciality_field_code_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.speciality
    ADD CONSTRAINT speciality_field_code_fkey FOREIGN KEY (field_code) REFERENCES public.field_of_study(code);
 O   ALTER TABLE ONLY public.speciality DROP CONSTRAINT speciality_field_code_fkey;
       public               postgres    false    4768    227    228            >   �  x��[�n�}��b�'	 ]I6� }l?�@�8@M���(@R����h�2��-F���P��4/C����_ȗt���9s;��\�U��p�}]{�}�ݵ��|��<K�Q2O�I/=���d���d����L�V�|���=y7�S�4��r�%�v�6qK�H�~��yn"�N���%���g��Ei#����?��y�,�_M��#�����If����v${���$ע�-{�
�/��>�?Kb��r}�+�}z�K�
��NY���V�� Ǒ��v{"�TW��規�d�i[�*"���\��G��;Ѧ��b�6�/��`���-W.(cCȻqz�t_~N^N.�Qx*O��:T|([5�����d�\!1c��˭#]D���B�<���My$N�r�L(���.4�pkQ��ɩ�t������x�<^�Q!.;[�)?;��x��/��Bdh�8�!�`��s�Zu�(1D�g�����3	wZ��LJF�Ml�%ZN�ZD��a:��3��6����8Bx7q��i�c��=L��Z��_7>x���7>�6�3�ކ)7j�������g>}���?�C$�,>���%th0N|"�a�BS�i1H�T#5шR:3$������L_$)�nZ�f�@�z�-���s�TS��݊���*F��%J=(t�`�.h�F4���"ũ�({��HY*!<
_0��ש�tX
q�h�\�������jQ�!�M�뾉di^� �#EE�,h�AF�b�P���sv�3�Y,��u�)�Ҽ��j$���M��+���Hc� ��\�K_��v�.���X��_��S���D�d��;ȣ� .o2�o�[e�e�/cٚ*�Y�`�s�h��%���߬̃|��,�G���=����f�D�T@q,��d����Yj���}=���RI�b n5g�uѰ�� �#��b�\)E��Y�H�W��$k�?KL�t�j� �	0����̂�fA¼�Kmس��mF���FDNѸXzS�&�h��QluoTF/;L05������B�$��Hc�iB{~uŬ����J����{��V��7�����I�BFYX���y�*�ݜ�&��gP#z�o�m��5�p����)����ީ"�/�E��^/jT�tFg���� ߶�,N�U�ax�(ħga���(o���n�g��8at��g�P�铊X��iA����8td�� P��hq�b��U�\�V�.�A�������$[�ʴ74`a�8IU�{��Gz>�Eހ5�|�̶U����Ls�zZ�^3ϑ(�Hi��3%���_Q������ݚ�s�o��;۵��tz+ھ�A���n��v��݊S���銴�%���r��[{"��J�]��zm�:����!�p`��uE��4D���Y��2��(�AL�Y5F��l�v��2�!|	EeA:����a���d�N-%m_	��,���=E~�ZE}�vLu1\���i��(%�ڇ�d�y}X��WWB~��W�Z2�~p���A���d�LG�'����)J��L;����&M�~�5yzA��S�T�Oz�ϭ[�d��� C�x�3�N�o�ܳ�����m���_}�>��h�8�1���|F�₽ס�%r���Q�qg&��NS6A�o��ʹ�#�������/��hR�7�V�XGX�Y���w�Q�ЄH��܌h	��֩�_��&���%��t ��p�QV ߖ��t-�[�%������:rڅ���'*���.O��������G��
t���1�4�#:S���3.A��,��[5C�f^��MG���M�M���9DY���� V1�`��tn;�]��0ƶ�N\;���L��233F�q@�[K�������y��g������!=���he�nG�"�@��.}s�1*7���i��L��x���%c%m�!c
�S	�4rWU*��,�O\��34]�5~6��\���A��̹8���M������Vy<��z�+JO���Sj����%��Z}ez���WV�����P���ڰ����bѹ�D(c�s�ߢ�XB� Y�#)�f�#X5��(,���-��
�9K-��;b֦!�������o�|"�v�G�����g�|��#��}���6�^>0�~V�{�;k�����SA�[>�u4G�LHD�淂C��)��V���u"ʐ)��%��-G�PST?�q��&����Q���:QD�*Ӹ�$(��n�J�(��=eG||Ho;���aXה.��|f���I�[�v=�m�j�ں�؇�+�W�T¥��t���O ��(d\=���i��?�[hC7NPV�u��]r�n
e��? ��K�<U���ܛ���)\u�O|�r� 7)�9Hy����-a���ÖW�ɝff.�~��$B�Yap_�霽c{��M�r6��@��qn\��AeicNCZe����!�D��	?��L��l������p�>����67V��;Z�U�c�s��P�����b����:W��fh�fu;�isk<�\���C7�\z@��U9��vE"'9�:g���ܨ:,�{+�G�iˋs"VU����h���e�ܹ�b�$�b�����W8�y�.f�M� 3�69�H���Ji��^8��B�<�!�Y�F�����	��%��%�W,S������S����Y�u���?q�t�l��x��2�W@F����4��8�PG<��Cx�Rt=rVꆞ��[ϵ�/�L�{p��ҋv��A�����6/An_U�o�[N��7m�t_�b�Q��/��I��2��O.�������7�y%���qu�U��ߌ�oF�Ys|o�6������W1W�%O��`�T���<M�I�|"oO��S��Z~��^	����剸�^�`�_�jç�(�`�v#�Q_��+��	*��떰yj�����CDe['p$#�Ё��W��#�ї�M��z�\@�9knkK�+g�9
�4Q�?p��:�u�U0G��r_�*mn;�s���ϖލe�4َ6l+��E�!O�pk}}�����      Q   �  x��UMoQ]��97@惚fֺk���
�M$"Ec����	ICA[�ZM��I�f���#ϻ#j��@�1�7��������L���GSфF�!�MeC�����Շn �uUVi +�%O�!it�׊R�'��-<�F� g���+��d�ƚeX��aƬ[�4��O���s#���>fs%y!��`�P:��E3���� B�VDޣ0��K�}C�y����|*W���!����y�"������\9W,��W�_e��D(�(�G}YS������gO
x�zG�.� �M@�F��Dnҙ�ً�5#P�Qe
�[�@p)z`3@T�l�
l���')d�2��[��#��a�\r�؈�����*A�^���^�AV������=?&]�)�Ti"�.[�uC��kf�N���V���n���_�g�E`��ТK�aS�Nn���Q&pr#f����lg�vl[K'#�f]�H'b��wñW�{�wu@gN���һr�32;��㎵Cg0C1 �`�d[����7�ƻ�Ȝ ���5����o�;1i�BÖC:�r\�KWq�o`�sƴ_�1*:cБk�f G�u�wCp���(��e�������k�t�����;�4$�"��$�bV�c3aƍ�͸�I�#���|�V      @   �  x��VMo7=[�b�)Е�_�-@z��5��`��R��rAR2�S%�0Ѓsj�@���C�����o��*9M!)��^�̒o�<�p}��ƚ�~��7��_���0�o���4��ȏ��I�U�#x�T{x;���!SY�tٷ��MG8Փkm�J���Ѕ��e֭wE#��E�E�(�OU�ԍ�螲JI�S��ȓR[�2"�
'��7~��^f���;`��e$'� �3x��w�@��጑W{~��!�r���s��"£O�Qէ�xi�j)���8����m��6/�����cU:~��n�x]ظ��h�JE�F�N�2~�,śm�k�����ŝ� ru �?���RH�OAe: 1�r>�{�ψ��(D�4�8n�����d���: ��9��WdS�RQ�e:�Nhd�s�� �,�c�*��&�0wsg�LY+�{�_�A�"sSRe�D��j\b'��LK���f�\����Zmib|��n��|�A�����}R�����>�@GDӍ�g�l�����j�qǦ̪� ��)2�#,4[���v�gy�ms���uGP�H<n�roA��d��U;����i�����ɠ`��Jcd!��f"m.�Toc�V&�|���� �0��_ �UP��/�������[�E|	:zO��v�������G�1�$F��%�w�W��rج�2��跥0A���Q�8.�(��\>��s��w�}?��v�M�G��a�?���Z��Pv���[	~Z�C��UB(�oԺ1�f5᪏��	��v%(��:�x�P���
'MO�m�Ѹu��6	M/a�6qey�xN݃H�WTl���1Nr�A��"�'��9�@�J0	\�l��U�ݤ�{�1=`H/P�CR�J�>
� �$s��L?�e_�~������D��N��X]�������^���MZ�      G   �   x�u�M
�0���)r���g��s#X���Bݸ�]	.��D�U�ԟ�W����"�./��7o<��^���j�3��p�[<
�?��$ɼ���tQx�-$��,
Sw�D�h������M��2v�Ʉ�v���^�$d�1\�@0sd�cKeg�oe'r+4d���t����%�V��z�V鼓X���9�O���      H   �   x�e�A
�0E��)r�"�����i�֍ .�^O����6�
n�oU]�a����t�p���+�������t�pB-���Bɶ�x�W\�3<�����R�L�aq?_�2�bz��gxcXK�=�p�-������G=U8"�a������4Rp:�JN�'q/S�:���;�b��~���      K   0  x�u�MN�0���)�	����=L�H�H�@�W��!!�fn�;�
;���7o<Fk���<5X%�:^P�c�j��5�N�6(UZ6�|R�y^ ���ِ��/�G`|���W8WTk���Z v������:D�������$��=1�I�t(��� v����TC�M]�G�٠=��9����
-�LI���5:�Z@����sY���H;f�� p؛��TvՏ�W'-f��kIǬdD��}�C���W��ƫ�&�bf�l�i����;���^�r7��>L�o���3�$I���s�      B   �  x����r�Tǯ��8ε�X���\r���S&�In��21��d(�u:\@q��ʎ��7�{$YR$��Y:g����vW'�E�Xm)�9�����P��o03��6��q��5��z@�L�(P��k}�Y �n]�F�;�܌�����>��w<�`t�����h�+�{u�'�,a����K5��ݷ���rM�mY�}������>��h�����������ޗ��a�B������KW�x�sߢ���ӈ�E.��MMhJc\Gr�Ua��.0x�s[�P�^�3���S	1��Q=�O$�X��h�a��
��~�5Cڙ��C�$Hk��_���EӿF����Ɉ������=�F�O/���ݥ�E�f�.w��8�'//����=N�	��/�����x�"�@�ʡ{�W=�I�?M�|�
��'���9E�	�)=��>�@Q���6�ٖ/ْ�ȯ��x�M��(��$vv��f�K�t�`��Ʋ�����Qͥ&o���s�BM��,��4�nV�uc�o�ʏ,J?����W&~�&%S}O�� ��$���^F�YCDپ��/�+.=x�6�׹������;ݫY�'ojx¢�(��ϰ�ɀ�{1@c�oI��N��9��/㛠�U`b��q7�m�r�g$�	�8��kq�"g���B��-s�-�_��WS������0�A�Z!5�3�o��-ω(�b����_K��W��s�/��/���id�-��,�i[�]ĥZ��\��cK8��9��V�X��~W�{*vF� d�R�[�r���0���A��>AS�`��I��m+n�l�Dzb1�5o٢�JU�z�t���Y/r&�مi^vM�y��	���4�a[X��Y��v�X'a�..nd"X�%Ǎ��B���x2n��)z���G��0r�avB	}h��v��H�z(�TTsȄ;�O�וqT��,V�W��쎣D�it�puW�WǠ�T�u\Q�q�[r��/�TH@y�p�0Qܠ�(�b��� �>/���XNn�|���N
�ka4���$��-x;��N�]z��E��S�`�X`������a�
�n9��Aҭ��$�FQ�Ga Ҁֻ����>�c:�A�����_�녿�����%�8�E���ٶ�?�Q��      M   �
  x��Z[O#�~6���ӌd��2�f�y���Qbp3x1nd�gE��l�0��D!E�n�(�s�m����������[UW�fQv��DH��]]u.�9�;���t�W/�;���(�nuǭ;������ղZm��S��p�	�0z�?U���8�g��OUS�1��;XyN� ��p�g��2��	|�h�^�f���K<$�ՓpO-�匿/J��nc�Q��N�@�]u��xjہߞS��_�MG�4�ZK5�]O��uۭF���rj�fG�����i��x��s�/�<��s�r��j��U�zJ��Nt�?��?�!��ʇ)R?W�=ܚ�c} z`_��Yԇ����%��	,��E��,z�����D��d@��_:;��m8�Mt�Fڕ�����)܃�/�h�)	1��;����ax�%U��#ѯ�~egJ���\� ]��P����΃ݧ�f��~t�ր�X!����LF� `�D�a�>������ޞ㶏4�P3�h���7
��������g�����|\c�6(Z%�J�k�ć[3�ā �wH���g	c�3�h��bm�^(�s2�9v��<:���1��3z�v�D�u�#P�J����BP�������짠 ڤےs����h��q{- h;�[<�E��T 4Z��o��l�؂�A�]w��B�`|ʇ+���0L9p�����;�$���e3`D�6*c+2f ڀ.љ�cשyݶ�r@��+m8��>j& �-a[��%��q
��� �P�#=�����G���Բ������t䭀Q�,�t
`� %F�:�:�,y�R\~H;��B�NV,
�㜴��/^c���f���vk;ݦw$M��˷W6�����Gpm:"�O	B=��Ĥ
*�b�4�tXby �Yy���6�=�����Ώ�/.|�[돕��=V����X��G�����'; Z���D��,NT��6�sqq��,��@��*��T�C������z��u���gȨ�\�(9�g��>'�t���T�oI̨V2 ��g_;.-�'�1(B��Ï��_p��� ���3�4&VIh�9E�I
O[��n�FěO�=U��2��@|�/��M�L�p�f�1&f��i!����ג��$��޹�S�F�Fp���>-'���q�α{��B���O�6́C��`�����ƥ1H�l@6 �kZpE�x�O@&/�(T��mW��N`=O "�J��kq��'HJ����gOR��/�FT�L��/�;�(@ fDi�d���2:/`[pG!��9���g!�W�:K�ie�Ң�lb��	H��q�	�l�h�d�+/�T��/�:Cv�%O�������9��`fIՏ�sT��J	B� @(V�ll$N�������
es�%���i�f��E?�,�c�����rn��2����i�_��Û���"�b(�� �R�4��Z�Q�S�t<44�|�X�� �R�A.L	'�/x�K�.� �5f�r���,�L2�"�9M��oA��Pٚ�B�D�5ir]֋�$*[�2�?�;TA&�Ros����^@#�_u��?/Sc
�~��>0B.�d+#����iN�8��Ǒ�D���D�hA� �&�+p1�|�á�1��	�L5�ƚ{O˂C<�2���B�(m��d#�X�	K��æ;H�r)�E�2agϳa��E�;�:,]/NJ,r�����9A�]���ii��Ȭ���fB*�2��j�8���T4����&}�8���n���3�)욳\#�:�9�2Ds=9d�t�E�j�\�hm�?M�p�� &�H4=��J�)ϭ&Lb�"�)�F�4�,R��cK����縂-/�]u*"�-��4so�>�*v�}p�d[�Z�QLb��ip+&�D��g1jjv��p`F��@!)��jJ���!z�T�Ѕּ��f����dL�� Hv�	ѳE#��bC��$-�{�S�~��(���g,Ly4oH�S$�V"�rL	HrE���yȘ�w�LS3� ������A.��F	2� ��XGnO���Fo�*�w~�?�S�7ɗ���.ׁ2�� 6�_vߙ&Xi�9�S*���k#�B���4F���^B*��Ĺ��V"K6rb[��J��S0dlIqR�(B9\��KRZ�.3m1D�S¼"o8LuM��P+�p�4�JKo4����H&03�D������*�)0l*��&=�)h^2Yii�ϐxZ>mREd���b�8���wϛ?����R���ӊ��!#'в��0�>38N��X�AaĄ�J���d�*r�(��n�0�֟U7>��o~�W���=�/�����	Cg�)@!���l���b���vƠ���������T՗��'���<A����X��Mej����%K���❕{�:α\�g�p���<R،��)~9�2Ux�d~J�6����5�4�-N"}d_�N�dn�d��¯=�*��"�1)6��{�e�f��e���18�:�#u��>g��c�ԫ,h��z˅+3,����F�]a����Ă�A0w���|z���g��D��hTt>y1�}��; L1�R�W�F�y���Ϋv�myݪS�V����+;��iyk�æ[�w�:��m�dms}�k���tj���W�^�m�/G��o�v�}�ثl���{��=��Z��aۭw��ƫJ���l7���|r��/�~���n��}�ۮ�wK��o�x���3qe3s�̕-�µz��_+_TWVV�>Ǽ�      O   �  x��S�N�P�o��a�P@��P'q āĠA����Hb"���8Wh�-�p����"�JMth��{��s~,Q?'���y�q!�M�R����ӄƦ\�h.#��h,J�V�9��=��W�;3�� �1"D ��+�S|���=��5x�	�%Crhġ�"��"�q��+�%�,R ����?� �1�c�$g�H�RYKH�&��.#��h$ʢ��cY�xeid�ƾ�S����x ����b�#F�A�C.X7�M�[�`�g��N��m5���|��Rg�x���Yǯ�2�`�*�rӨ̲e����Xu~Iw\�d�2��SL�s�6��SUcz�v�&��%���������4�-u��^ڀ���R�1W�檴��֋ǁ��g/�<"y#\^�lyV��]u[�m�w���N�����_L��b�%���� ��ӗ�{q-�_4�?���a|�#�      D     x��VKn�0]����V�q�d�,�k�`Ǳ�F��jZ�����W�(oF�%r�fS @H������Цݸ����{~�����i��?�A;{M����4��RZb��#M��Q��j�y�ц��KP�L��ozg9�=��`�5o�����w���lh��i�1?��GZ(�4s����Y!�bH��~�oez����8I�6��1�D�w�#;���!��"!���|r�q���&���J$���w��΅��L�Y��T�+��I!؎e�;��q��1��2���+���風v�0��N��~��8׉8ǘE�vkQ���h����}���	���nw����<8��ym����@
54dS2��f�T��L#`��v4/WʎU�B����*<D�#;�*T�Q&��V̛��0w��KJ�`��&v
�dFp/兀���.�>�p����)�吰J(f1A�	����^��S�)Uc�X����$c��c}-���
T��!.�� �=�.Q@h�%�)#L�&r��?�$-��oʆ;���P�@&{3S�6Qe�
��u�:W�ĕ�����-��H�������k���[��1U
�R(�$|$UY��Q�k �]Զ�W�ր^+�W�w�?��Z��z j	��o��=^u��J��Y��J�U�yΜ5w��vn:���٣�9O���4Jt�v�P��s@vF�V�e1p#G��7�WUZ[���E��Wi����x��+����f,Q>4xn�u�f�h覇�!����ٲ v �m��"��Ch>�o�RO4H�      I   �  x�uR�N�P\�~�]jbK�C�O�1���؅X6&&&�K�Z~�?r� ��(�=gΜ�3�F^��Fj<�qN�؛��I�j!�|Ɂg�[�Kb�\��@J]�W�D+��L�ɭ�we+{�����'���R�QG����k�g:� �U�!�O)	�+)0R���>2�!���K�t�j�w�=tuce�trEhѥ��aJ]a���X�a<p�n )��L��O�j�˺N�������{Hqp	]��/��K��|M����Q��t�u��v�8�+g�R��h��0"�`�:�r�@�/]�dA7儲�L��2@Z��I5�͍�����������)�u���l�>��Ǽ6E�Ѻ��ߙ�pօ�7��-ct����;0X7I #��}��<�����     