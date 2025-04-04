PGDMP  9    %                }           ontu_phd    17.2    17.2 K    S           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            T           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            U           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            V           1262    18003    ontu_phd    DATABASE     |   CREATE DATABASE ontu_phd WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
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
       public               postgres    false    217            W           0    0    applydocuments_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.applydocuments_id_seq OWNED BY public.applydocuments.id;
          public               postgres    false    218            �            1259    26290    defense    TABLE     z  CREATE TABLE public.defense (
    id integer NOT NULL,
    program_id integer NOT NULL,
    name_surname text NOT NULL,
    defense_name text,
    science_teachers jsonb,
    date_of_defense timestamp without time zone NOT NULL,
    address text,
    message text,
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
       public               postgres    false    236            X           0    0    defense_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.defense_id_seq OWNED BY public.defense.id;
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
       public               postgres    false    219            Y           0    0    documents_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.documents_id_seq OWNED BY public.documents.id;
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
       public               postgres    false    226            Z           0    0    employees_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.employees_id_seq OWNED BY public.employees.id;
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
       public               postgres    false    230            [           0    0 
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
       public               postgres    false    221            \           0    0    news_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.news_id_seq OWNED BY public.news.id;
          public               postgres    false    222            �            1259    18181    program    TABLE     `  CREATE TABLE public.program (
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
    results jsonb,
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
       public               postgres    false    232            ]           0    0    program_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.program_id_seq OWNED BY public.program.id;
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
       public               postgres    false    234            ^           0    0    programcomponents_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.programcomponents_id_seq OWNED BY public.programcomponents.id;
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
       public               postgres    false    223            _           0    0    roadmaps_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.roadmaps_id_seq OWNED BY public.roadmaps.id;
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
       public               postgres    false    225    226    226            �           2604    18135    job id    DEFAULT     `   ALTER TABLE ONLY public.job ALTER COLUMN id SET DEFAULT nextval('public.job_id_seq'::regclass);
 5   ALTER TABLE public.job ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    230    229    230            �           2604    18050    news id    DEFAULT     b   ALTER TABLE ONLY public.news ALTER COLUMN id SET DEFAULT nextval('public.news_id_seq'::regclass);
 6   ALTER TABLE public.news ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    222    221            �           2604    18184 
   program id    DEFAULT     h   ALTER TABLE ONLY public.program ALTER COLUMN id SET DEFAULT nextval('public.program_id_seq'::regclass);
 9   ALTER TABLE public.program ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    232    231    232            �           2604    18208    programcomponents id    DEFAULT     |   ALTER TABLE ONLY public.programcomponents ALTER COLUMN id SET DEFAULT nextval('public.programcomponents_id_seq'::regclass);
 C   ALTER TABLE public.programcomponents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    233    234    234            �           2604    18053    roadmaps id    DEFAULT     j   ALTER TABLE ONLY public.roadmaps ALTER COLUMN id SET DEFAULT nextval('public.roadmaps_id_seq'::regclass);
 :   ALTER TABLE public.roadmaps ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    224    223            =          0    18004    applydocuments 
   TABLE DATA           `   COPY public.applydocuments (id, name, description, requirements, originalsrequired) FROM stdin;
    public               postgres    false    217   �V       P          0    26290    defense 
   TABLE DATA           �   COPY public.defense (id, program_id, name_surname, defense_name, science_teachers, date_of_defense, address, message, members, files, date_of_publication, placeholder) FROM stdin;
    public               postgres    false    236   �b       ?          0    18010 	   documents 
   TABLE DATA           D   COPY public.documents (id, programid, name, type, link) FROM stdin;
    public               postgres    false    219   �z       F          0    18102 	   employees 
   TABLE DATA           @   COPY public.employees (id, name, "position", photo) FROM stdin;
    public               postgres    false    226   ]~       G          0    18112    field_of_study 
   TABLE DATA           4   COPY public.field_of_study (code, name) FROM stdin;
    public               postgres    false    227   1       J          0    18132    job 
   TABLE DATA           :   COPY public.job (id, code, title, program_id) FROM stdin;
    public               postgres    false    230   �       A          0    18020    news 
   TABLE DATA           e   COPY public.news (id, title, summary, maintag, othertags, date, thumbnail, photos, body) FROM stdin;
    public               postgres    false    221   8�       L          0    18181    program 
   TABLE DATA           �   COPY public.program (id, degree, name, name_code, field_of_study, speciality, form, purpose, years, credits, program_characteristics, program_competence, results, link_faculty, link_file, accredited, directions, objects) FROM stdin;
    public               postgres    false    232   ܅       N          0    18205    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, program_id, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    234   ��       C          0    18041    roadmaps 
   TABLE DATA           ]   COPY public.roadmaps (id, type, datastart, dataend, additionaltime, description) FROM stdin;
    public               postgres    false    223   _�       H          0    18119 
   speciality 
   TABLE DATA           <   COPY public.speciality (code, name, field_code) FROM stdin;
    public               postgres    false    228   ��       `           0    0    applydocuments_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.applydocuments_id_seq', 1, true);
          public               postgres    false    218            a           0    0    defense_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.defense_id_seq', 2, true);
          public               postgres    false    235            b           0    0    documents_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.documents_id_seq', 11, true);
          public               postgres    false    220            c           0    0    employees_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.employees_id_seq', 3, true);
          public               postgres    false    225            d           0    0 
   job_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.job_id_seq', 10, true);
          public               postgres    false    229            e           0    0    news_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.news_id_seq', 8, true);
          public               postgres    false    222            f           0    0    program_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.program_id_seq', 1, true);
          public               postgres    false    231            g           0    0    programcomponents_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.programcomponents_id_seq', 11, true);
          public               postgres    false    233            h           0    0    roadmaps_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.roadmaps_id_seq', 13, true);
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
       public               postgres    false    4773    234    232            �           2606    18126 %   speciality speciality_field_code_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.speciality
    ADD CONSTRAINT speciality_field_code_fkey FOREIGN KEY (field_code) REFERENCES public.field_of_study(code);
 O   ALTER TABLE ONLY public.speciality DROP CONSTRAINT speciality_field_code_fkey;
       public               postgres    false    227    228    4767            =   �  x��[�n�}��b�'	 ]I6� }l?�@�8@M���(@R����h�2��-F���P��4/C����_ȗt���9s;��\�U��p�}]{�}�ݵ��|��<K�Q2O�I/=���d���d����L�V�|���=y7�S�4��r�%�v�6qK�H�~��yn"�N���%���g��Ei#����?��y�,�_M��#�����If����v${���$ע�-{�
�/��>�?Kb��r}�+�}z�K�
��NY���V�� Ǒ��v{"�TW��規�d�i[�*"���\��G��;Ѧ��b�6�/��`���-W.(cCȻqz�t_~N^N.�Qx*O��:T|([5�����d�\!1c��˭#]D���B�<���My$N�r�L(���.4�pkQ��ɩ�t������x�<^�Q!.;[�)?;��x��/��Bdh�8�!�`��s�Zu�(1D�g�����3	wZ��LJF�Ml�%ZN�ZD��a:��3��6����8Bx7q��i�c��=L��Z��_7>x���7>�6�3�ކ)7j�������g>}���?�C$�,>���%th0N|"�a�BS�i1H�T#5шR:3$������L_$)�nZ�f�@�z�-���s�TS��݊���*F��%J=(t�`�.h�F4���"ũ�({��HY*!<
_0��ש�tX
q�h�\�������jQ�!�M�뾉di^� �#EE�,h�AF�b�P���sv�3�Y,��u�)�Ҽ��j$���M��+���Hc� ��\�K_��v�.���X��_��S���D�d��;ȣ� .o2�o�[e�e�/cٚ*�Y�`�s�h��%���߬̃|��,�G���=����f�D�T@q,��d����Yj���}=���RI�b n5g�uѰ�� �#��b�\)E��Y�H�W��$k�?KL�t�j� �	0����̂�fA¼�Kmس��mF���FDNѸXzS�&�h��QluoTF/;L05������B�$��Hc�iB{~uŬ����J����{��V��7�����I�BFYX���y�*�ݜ�&��gP#z�o�m��5�p����)����ީ"�/�E��^/jT�tFg���� ߶�,N�U�ax�(ħga���(o���n�g��8at��g�P�铊X��iA����8td�� P��hq�b��U�\�V�.�A�������$[�ʴ74`a�8IU�{��Gz>�Eހ5�|�̶U����Ls�zZ�^3ϑ(�Hi��3%���_Q������ݚ�s�o��;۵��tz+ھ�A���n��v��݊S���銴�%���r��[{"��J�]��zm�:����!�p`��uE��4D���Y��2��(�AL�Y5F��l�v��2�!|	EeA:����a���d�N-%m_	��,���=E~�ZE}�vLu1\���i��(%�ڇ�d�y}X��WWB~��W�Z2�~p���A���d�LG�'����)J��L;����&M�~�5yzA��S�T�Oz�ϭ[�d��� C�x�3�N�o�ܳ�����m���_}�>��h�8�1���|F�₽ס�%r���Q�qg&��NS6A�o��ʹ�#�������/��hR�7�V�XGX�Y���w�Q�ЄH��܌h	��֩�_��&���%��t ��p�QV ߖ��t-�[�%������:rڅ���'*���.O��������G��
t���1�4�#:S���3.A��,��[5C�f^��MG���M�M���9DY���� V1�`��tn;�]��0ƶ�N\;���L��233F�q@�[K�������y��g������!=���he�nG�"�@��.}s�1*7���i��L��x���%c%m�!c
�S	�4rWU*��,�O\��34]�5~6��\���A��̹8���M������Vy<��z�+JO���Sj����%��Z}ez���WV�����P���ڰ����bѹ�D(c�s�ߢ�XB� Y�#)�f�#X5��(,���-��
�9K-��;b֦!�������o�|"�v�G�����g�|��#��}���6�^>0�~V�{�;k�����SA�[>�u4G�LHD�淂C��)��V���u"ʐ)��%��-G�PST?�q��&����Q���:QD�*Ӹ�$(��n�J�(��=eG||Ho;���aXה.��|f���I�[�v=�m�j�ں�؇�+�W�T¥��t���O ��(d\=���i��?�[hC7NPV�u��]r�n
e��? ��K�<U���ܛ���)\u�O|�r� 7)�9Hy����-a���ÖW�ɝff.�~��$B�Yap_�霽c{��M�r6��@��qn\��AeicNCZe����!�D��	?��L��l������p�>����67V��;Z�U�c�s��P�����b����:W��fh�fu;�isk<�\���C7�\z@��U9��vE"'9�:g���ܨ:,�{+�G�iˋs"VU����h���e�ܹ�b�$�b�����W8�y�.f�M� 3�69�H���Ji��^8��B�<�!�Y�F�����	��%��%�W,S������S����Y�u���?q�t�l��x��2�W@F����4��8�PG<��Cx�Rt=rVꆞ��[ϵ�/�L�{p��ҋv��A�����6/An_U�o�[N��7m�t_�b�Q��/��I��2��O.�������7�y%���qu�U��ߌ�oF�Ys|o�6������W1W�%O��`�T���<M�I�|"oO��S��Z~��^	����剸�^�`�_�jç�(�`�v#�Q_��+��	*��떰yj�����CDe['p$#�Ё��W��#�ї�M��z�\@�9knkK�+g�9
�4Q�?p��:�u�U0G��r_�*mn;�s���ϖލe�4َ6l+��E�!O�pk}}�����      P      x��][o[G�~���@$H]l��w�iv���m��d׋5&����� �(�`X���ے-;���x�h��y����%[�U����M�In��D��V]�uUu�f��Q�UU���WRgY�������Z���Yu�:����6}��ޖWQ5~�Qe������U�ۥmz��U����핼��=����'�xH�U]���k��GU��JYnD�*o�>7�^�A�T��f�6��N�<���R�����V�{HE�ş�/o?�����K��ԭr��Th�ʯP�t�V��AkS�Ԋ,���_���㖷'*�6}����W���+�g�
k��Znm3[ܸU(пd��BlS��,�}�E�S��;bj�G%����A��f#�S���������n��
:W��:y�6��#�.���Anm}����zMO��ы��zY�S�<�!�Q���ƢJU1���ĳѕ��Hj��1�Ч��<O4%�~U*w�[��������ھ'B¢��_16;zl?�0����S�uG���}8ײ�O�	��B�:�P�H}]�C++�<�T��dK�^���P����X슼ֲ�p�:�*\"�}�{�-����7�����vd@xTY�Y��-*�fV�/�E|���~D�_�
^���\�:�B��T�jV7]���>�B���t��#�?��Ecv�^Ͳ�_g��?+�|�O_���+���ᳯ��곻�Ї�Q�����o��ٿo=eQa��+zziia�&�?�O�au3#F�\'�K��R�]��jC�@�]�~E$<�j�mXu�,Ĺ�㨡��:#ȠW�x�Q
��V��J+�ٕ�}�;���?ߣ����m��w?��2��'�{)�%��N�Z���Tؿ|�����~}%=�m���Z��jv~���%ST����k��{��SmU���O{��]
��uj
������1Tm�D�|��M
_�a}��0���p5��蕿����F��5�K�w���cj /�*P�_��H.�TH�Xb���;���hM�ʌ�JSk|;(����bV�~��_�w̳��1�ӆw_&���ŕk� /�yZ!G�ybKΆ��;=�
��{����P����z֛�y���ӹ�ŘmC=��̴1�E�{���g�툔{���^�|X�f!�`��+�y��F��
-�~j�Q�ؤHk15�ˍ>V���.��tl��&�?�r����B[I0i�qD=j���Lv��1r����R�6XXrY��
:�Z�!
@�+�Ċ����`0d�;КÂ(߬��On�}_MgV}�*vt�ن�-�k�.�Z�O^�P���c��� �����s�%{�������w����>d����2t�ۭ��>Qdy�Y���tK	al%m��BCu~��ߠ����w�}{G�lx��(ѡz9�^�8�X�C�$�S�o����u�od��i#�F
�?ד���xnG)������&������i�1n�S?_M�b����܃���v�G��Q^�Y޼�� C�h�?ݹ�o_~��BC�y��~D���4�	J��8Ϋ��ly�sn���ڥ���P_rA�j7�S��e�E�>��ݵ"s}/�͗�f#��`/yS�t��GmK��ߴ��:+��L;�M�-ڪ�L8�Z�:�������� �h�5�A�	<檺L*�N7h�1*M�c�D�%Mqj�!&#��)��D�X�%�����W7]"P���\a#����e�S`˶ �E<�L�Y�5՛�B�,1k^c|kV���u�fL�m6`li�5,��X��.��[��]x[�*���U�-�%T�gY��"�h�/��:F�7H`&��!C�˖�h�FfF�MUF�t2ray�� �}1�w��Ty!6��g�&�fXS�i�{c�r֑2D�,��mXqPg�b��|NON�qFH���_�>�GZ?�x�x:D�H�B0�S徦$"�Hl!jI������yMC���R誡#&->���U��i4U^����^l������3Qw�_���G�(�`�Q�7�0��)/u��C��J�zD㶢o�h�a�ЗX_����z�F�ү�`:]a�lO��;C���03�9��y���o[�Q�C�����Ȩ
����B�-�e,��\�z�FX�lf"lԙf���,�j���5jp�ֺ���͸mFO�|}�����zL��3Oq��������&>N�T&�l�Ծ˂f?H�ʖb?H�C�qI�Tя� ��-0���K�c�G�x�GZ�W삉᳐a+��b}��"�`?sv��G(y���)}�����/�����qľl}���p�nB�e�6M�� �����M�0����Ǆ���$y����k����Íb�p����k��ު�^��*���?�B׏�8dȗC[����4�8����q?#q=���4���|��0�S?�񸧕~�K���d���ۀ�E��a�x�'��c�F�F�f����Wa8�0�0��}�$ >`�A���&LJc7��6�@�v�	��Q�/�� ����~ߘ����0`����g�׳�!s����8�ի �i�.�٪�������%�r�=i���I"raӆ��k���uDR\.������{@�G�5��>Bz&Ր�?�p������p8:}��b�B@����-�Lʧoٻ����h�'��ӃON���v�:��c��4�1P5�U�h��	�v�`zÁ��y ؉8�<r�]��.��-�ޭ�'ǋS,��_�SULӑ�#)b}��T�0w�3�k`���Q��[/��D��Ut|�֧��qǥ�ҳ�e5qVv1:"�B�!R@�&��!�}B|�����1w�(8�pJ��-ܐ��9�Y�a����0!��?ѓ�t��7�t ���DYSSd�1�w+��?y�l>�LC�c��V���C�׭%��rh�8�����<%�#�#�\$��S? �[(і�h���Q���S��x�p� z���d�:IN�x(��B1XJ|��mu3$p.�ji��"k�Ꞩ��L%��%}�Sҧ��tEb�N��!��t�}����i.viV22Q���VK�Le��4�I�gu�]���ો�$�f�=q9�Tc���P�z���%z�Lw�7�L�&�P��
>�3P:]H۬ƃ�l؛���xʥ)M��n�ap�X�c�L
��>��[�p�֭��$-m[{Z�r�b��y���YO�脵�Uv��gZ#F���Y��R�t&�
�o��̆��[�,�Qb'P��&����h�-X���O������6fw©}��ľ�h�VqZ'�����4�|	ZJ�����m�\�t�.���s5H�0��2��<7�yY>�ť�vl�2[]���(d�",-��h���xW��q��#k��5��¼����ܰ�٧��$�w?��r�fͿM5��A����&�3�3�wA/�"��ٚ��������>ű4���`����!Z7�fU�4���(M����A��'�����!$O���:C��j�֒^�����.�S�&�����=d=��匵,��=t�K�ŏ��������S��3Y�~�c�����t$���Me�ف�����s���T�h�ۇj:��-��m{��T��S���1B�r��c����%4�N�[��h��V"���=Ȼ���M�*Ix���k�����ى����@�
�݈D��]��2WlN��I\�3���R�B'4� �du��O���N�K}L�ad�e�4a��=��i,cpo�	i�^�iz'���]���!t�Zʻʆ̪Ͼ�Ҵ�ْj���v!^>�i�Ջvb(��t%�0������c]�8r�ʄ�o���g�z�P�7�ڟ,��S˘c�&��4�`SD�u�Da��̝ܹ�M�B��G+���޳",7�:������N��oj������5�S�A���i����t�Ѻ���G�nc�5S�6mmc�ZW�:i쫧�������8O���P�uw��B�FUYA7�?���#7�����^����p���n3�� �  �<�f�5���E��wZ~�e���*���fݔM㮿�0��R7٦�wǳ%�)�f����VN�NOW��?㕻�~¡[�E�&m���=�	�F7M�Gy��]Fխ�W��3hI�xӊ�yU���W�GRwx�r�1���y�|4�r0ע��:S�O��} cz��Z����y|��.�D49V�d���Op�F?8G�{��mI��/-��e<��ׂ��Uݲ�j���OG}:��Ex�QZ�5钋�t�奡�>-�r�	���\]�KK���+]�%�//��ܴ��q��9���T��4c����ʫ�<I��t̛��`�G�"�scaG�-����󡌕��;A>b��0[4�gU��B�+���
�X�x"�5�4؅L�k~�ع$�T�-;�6��QW#ܥ�;a��Vd�m�˷��9��֋����~D�r�:&r)y���D.�ݨ
�P'b����}2w�x@;g�Ǡh�J"vئNw/�b�bj2՛*%Zy��i2�eDx�>�	��r_���O���KOb8�+`��C�no*���ιu��y�Ao�v��V�3ڒ� ��|B�l��ʞ�.m�sc	�R2���94�{PƄ��Z�����K�ՅKf��l���R�x�v�$���ϧhi��#Xft�I�����|�3(�;�R�k~:�ځ#$!���Q^���ozcx�M��,-V�Zs�%���q> 5A8󅚀��c��_Jr�)ɗJ��Հ�ɔ��;�ҽ+�e%���zV����������+p��s�6#~��s�R��Ig����'�	+�Y��d���(&�R��'�񓎟����rQ����p�b0O�kո�i��
���Qc�w�ߗqbǤ�����ňB���~��\���-�XL�ξ� F�����?{��#(E���!y]eT7
f?kb}��~R6��D4X��ܨ�Ŧ���E,(�
�_�a����6�������0ux��B��C�ı��ռ$�fl�\��P��gF~���`Nӂ?W�ټ.7��;���z�{�G9�5C#����ք��R� 5X��r%�;B+�*bDMJ4�-��%�cJ'����I���tƭ�ր��-�����NGu^Z�ӹbN���.��3���1���`�L�Ə�J�Z1_���s��8�z��[��I���onA���}}:)6r\!�ڏ��pg|�g��g���Y�$�#�Ћ]��</-���h<�	��m��"!��˘�H�~6W�|*�1U ��X��`�XT�����f"z|
���'��-��r�t�SN�9���#d'�a#��C�3��c��M���704��R ˧h�-VNO��WIރCz��$��ǐ������O���Ƌ��~�X3vҷː�P�������@tL���eBf4�!G��͝#���''5G�)��A�I�NR�`�i <�cJW��2�3�~by���=�A�f�' ���]d'�v�=O�h/ee/���Y��s�ڏy5��	̏{�q�V[4���@����ܾw�s�/U_7���'�Hy5�0(��lg3C��J0Ok�7�A�Ν�XG�jrÏ�{)�*f�9�u��(��<�B'usBOv�uA��U9�`*�H�֊B����v�?�=G`=�:ه.(�-ux��'J2���J�D��ⅳ�$�8v�����|qcß�Q�쓚$�:�5�����)0�S^�X�2��T�bB���vņ7	c	{�Yߢ�����@	���6f��(��R\ä�ȋ?�a��CQ+�R�F�����]�TU!�u`�g���]�y�Gu���}BsO4�sM2~�c!�l>�ɧ�| ��諙��-����fM/fvt����Xx��4{8T&Z�^�ݔ?KJ�p$�sW�$�\\9Yhvl^,�y7@q��h��SuU�р�b��Eh���k׮�?��ȿ      ?   �  x��VMo7=[�b�)Е�_�-@z��5��`��R��rAR2�S%�0Ѓsj�@���C�����o��*9M!)��^�̒o�<�p}��ƚ�~��7��_���0�o���4��ȏ��I�U�#x�T{x;���!SY�tٷ��MG8Փkm�J���Ѕ��e֭wE#��E�E�(�OU�ԍ�螲JI�S��ȓR[�2"�
'��7~��^f���;`��e$'� �3x��w�@��጑W{~��!�r���s��"£O�Qէ�xi�j)���8����m��6/�����cU:~��n�x]ظ��h�JE�F�N�2~�,śm�k�����ŝ� ru �?���RH�OAe: 1�r>�{�ψ��(D�4�8n�����d���: ��9��WdS�RQ�e:�Nhd�s�� �,�c�*��&�0wsg�LY+�{�_�A�"sSRe�D��j\b'��LK���f�\����Zmib|��n��|�A�����}R�����>�@GDӍ�g�l�����j�qǦ̪� ��)2�#,4[���v�gy�ms���uGP�H<n�roA��d��U;����i�����ɠ`��Jcd!��f"m.�Toc�V&�|���� �0��_ �UP��/�������[�E|	:zO��v�������G�1�$F��%�w�W��rج�2��跥0A���Q�8.�(��\>��s��w�}?��v�M�G��a�?���Z��Pv���[	~Z�C��UB(�oԺ1�f5᪏��	��v%(��:�x�P���
'MO�m�Ѹu��6	M/a�6qey�xN݃H�WTl���1Nr�A��"�'��9�@�J0	\�l��U�ݤ�{�1=`H/P�CR�J�>
� �$s��L?�e_�~������D��N��X]�������^���MZ�      F   �   x�u�M
�0���)r���g��s#X���Bݸ�]	.��D�U�ԟ�W����"�./��7o<��^���j�3��p�[<
�?��$ɼ���tQx�-$��,
Sw�D�h������M��2v�Ʉ�v���^�$d�1\�@0sd�cKeg�oe'r+4d���t����%�V��z�V鼓X���9�O���      G   �   x�e�A
�0E��)r�"�����i�֍ .�^O����6�
n�oU]�a����t�p���+�������t�pB-���Bɶ�x�W\�3<�����R�L�aq?_�2�bz��gxcXK�=�p�-������G=U8"�a������4Rp:�JN�'q/S�:���;�b��~���      J   0  x�u�MN�0���)�	����=L�H�H�@�W��!!�fn�;�
;���7o<Fk���<5X%�:^P�c�j��5�N�6(UZ6�|R�y^ ���ِ��/�G`|���W8WTk���Z v������:D�������$��=1�I�t(��� v����TC�M]�G�٠=��9����
-�LI���5:�Z@����sY���H;f�� p؛��TvՏ�W'-f��kIǬdD��}�C���W��ƫ�&�bf�l�i����;���^�r7��>L�o���3�$I���s�      A   �  x����r�Tǯ��8ε�X���\r���S&�In��21��d(�u:\@q��ʎ��7�{$YR$��Y:g����vW'�E�Xm)�9�����P��o03��6��q��5��z@�L�(P��k}�Y �n]�F�;�܌�����>��w<�`t�����h�+�{u�'�,a����K5��ݷ���rM�mY�}������>��h�����������ޗ��a�B������KW�x�sߢ���ӈ�E.��MMhJc\Gr�Ua��.0x�s[�P�^�3���S	1��Q=�O$�X��h�a��
��~�5Cڙ��C�$Hk��_���EӿF����Ɉ������=�F�O/���ݥ�E�f�.w��8�'//����=N�	��/�����x�"�@�ʡ{�W=�I�?M�|�
��'���9E�	�)=��>�@Q���6�ٖ/ْ�ȯ��x�M��(��$vv��f�K�t�`��Ʋ�����Qͥ&o���s�BM��,��4�nV�uc�o�ʏ,J?����W&~�&%S}O�� ��$���^F�YCDپ��/�+.=x�6�׹������;ݫY�'ojx¢�(��ϰ�ɀ�{1@c�oI��N��9��/㛠�U`b��q7�m�r�g$�	�8��kq�"g���B��-s�-�_��WS������0�A�Z!5�3�o��-ω(�b����_K��W��s�/��/���id�-��,�i[�]ĥZ��\��cK8��9��V�X��~W�{*vF� d�R�[�r���0���A��>AS�`��I��m+n�l�Dzb1�5o٢�JU�z�t���Y/r&�مi^vM�y��	���4�a[X��Y��v�X'a�..nd"X�%Ǎ��B���x2n��)z���G��0r�avB	}h��v��H�z(�TTsȄ;�O�וqT��,V�W��쎣D�it�puW�WǠ�T�u\Q�q�[r��/�TH@y�p�0Qܠ�(�b��� �>/���XNn�|���N
�ka4���$��-x;��N�]z��E��S�`�X`������a�
�n9��Aҭ��$�FQ�Ga Ҁֻ����>�c:�A�����_�녿�����%�8�E���ٶ�?�Q��      L   �  x��Z[o#I~v~E)O3��\&Y��q�������3��qG��*��l'����D��]B��nۉs����	�έ��o���.��������;�|�t�����j�R����_/n�Ug�Z\^_,��Fe�>	&��`�7�S����`��*�T9���/����$���
n����6�C�1�o�`�S���`���z����x�y��ڪm:�����}��v;���p�w۩���ܺ�6�N����V[mnw��Z��4jm��Vj��^�c�Sw��n{����n�ֶ���,�˅���:(Ui:����.�+� �H�L�pk�@�>��a<��a>^ѷO���������
�&���O�m�n|�l���o��/ó�*�Ӯt�DGN��.�}ZDOH�1|_���GÃ�(�� �5�~��+�8r���S��+P
տ��<�}k�*�h،��x������`x�PO�v졙H�һ����};Ԍ4��>��0�=�n9�Yk81|�D8W���@��Vʴ�@��4���T<�� H�-���i�ظ�T4�s�X[�S����u��4�`p���]F8s�����lk$�Tk���ή�h��G� ڤےs����h2�p{� h;�[<�E+�T 4Z��o��l�؂�@�-w��B�`|ć+���0L9p�o���[�$���e3`D�6*e+2�ڀ.��c˩�;M�4�'�p?=�L N�ö�3��<{KA¡�Fz�q7�s#�9ĩeC;:߉�o��+�Y���ò>J�6�u�t�Y�8�K;��B3�NZ,
�������_a���z����*��z{_>��Na�o/�Pm��#����tH���`�c4�IT�� i6�)���r_�1���k��M	z^c��
_T�Vז*�C�{�|���}�ʷƕ��v6@��kͧ���Z�0���wq~��,��@��C*D�C���c��z��E����Ȩ\�(9�g��'�d�.�T��H̨V2 �dg_;.-���1(B��Ï>���q���!���S�4&VIh�9E�I
O[�.�n�R�O�=U��2��@t�/��M�H��s�f�1&���i!����ל��$��ޙ�S�FՆp��>-#���q�Α{��B��݋�6ȀC��`�����ƹ1H�O6 �kZpN���O@&/�(T��m��N`=�"�%J�c�kQ���HJ�����OR��/fFT�L��/�;W(�/fDi�dG��,<�a[pG!����g!�W':K%ie����tb���I��Q��t�h���++��T��/�:Cv�%O�������9�3�$�G�9��`�!��{+b66��AY��[���@��9ǒ�\�4y�BƢRL��d���hyc��)2����I�����ˢ��"�"(�� �R��5�䰚�Q�Q�t�74��|�X�� �R�ANM	'�/x�K�.� �f�b���,eL2�,�M˷oA��Pٚ�BG�5ir]ԋ�%*s[�"�?�;TA��Ro3�y���C#���u��?+Sc
��e���˅5�JIiw1v�?�P���`"GAp(a4#b�Q�ڕ8u��������r�Mcͽ�EA�!s���IV!A�6Xz��l���������ҳ\Ju��T���7���"��B��'%9���B�����NQA�4I�YdVs�I3!���`e5h�Gg�
q�h3a�C�~X�7���\�v�i��f���	���2S�ʍ"yQφ	��c�֏R��$�h8RLcD$��]sK%���Vc���RF_�dQJ	)�r���fb`߿r\��g�:�=�KW�o�>�*v�=p�d��Z�QLb��ip+&�X��g1jjv�p`F��@.)��jB���!z�T�Ѕּ��d�م�dD����w�1��E#��bC��$-�{�#�~��(���g�Ly4oH�#$�V"�rL	HrN���y}Ȉ���L3� {���n���3��]>�0dJA4=����Q{QߔUv��8{*����/%R]�}e��~d6��cM��D��N�Ȏrt���Qn��!b3D\�1��Z�g��[�,�ȉmytv$��G���%�I�V��p�6�Ii]�̴��SL!,�I� 1�5)+E�t�!Ӱs-�|��+
�"���Hb�O����#��ok��8�)���d'�yIe��?�iٴI��|⊉���7��ͫ�������CFN�EE�ah}fp�rwk��#�	�26#cɨ5P��al�+�da6*,��W���W?īו�Y�K�t*�x��i
�ˠ��J���jh�:mp�Oy�xALQ}k}��L���I�<�T&����D����_�,^[�'�����W^z�
���#��蝝�S/�Q��L�'4k����QsI�����G��h�M�6s��)�����Z��1�bnC�n�%]n�l6�l1�8 �!GPĠΟ#�����xLA=�z�]Qo9ae��q}��V�(�͍�.�A�X�"��޲�G���bC�ϒ����H����"/���b~D�)�\YJ���Ո�v{���R�e��6ڝ�S�;��/�J���i��:{u�Rm-�jm�����������חtj.����ví�/j��ү�f�}Y�.m��{���+�W��^ӭvvڵ��Zi��ݨ���]���k_��Z��ِ�; ^�YޫnژV�f�A:�;�ؕ�ԕ'�+k��+�J�ֹ�9�f���������.y�4�,#����K3���Y./��U�i�����/�,��߉'�x�+�ug	J� �.�7�	<�B�d��hK�x�/}MFa�RҜ˰$=��<��Y{p8�����x�qV��d�:�)9�'�N Τ!�Sѹ�a����C|��?//,,�9(+
      N   �  x��S�N�P�o��a�P@��P'q āĠA����Hb"���8Wh�-�p����"�JMth��{��s~,Q?'���y�q!�M�R����ӄƦ\�h.#��h,J�V�9��=��W�;3�� �1"D ��+�S|���=��5x�	�%Crhġ�"��"�q��+�%�,R ����?� �1�c�$g�H�RYKH�&��.#��h$ʢ��cY�xeid�ƾ�S����x ����b�#F�A�C.X7�M�[�`�g��N��m5���|��Rg�x���Yǯ�2�`�*�rӨ̲e����Xu~Iw\�d�2��SL�s�6��SUcz�v�&��%���������4�-u��^ڀ���R�1W�檴��֋ǁ��g/�<"y#\^�lyV��]u[�m�w���N�����_L��b�%���� ��ӗ�{q-�_4�?���a|�#�      C     x��VKn�0]����V�q�d�,�k�`Ǳ�F��jZ�����W�(oF�%r�fS @H������Цݸ����{~�����i��?�A;{M����4��RZb��#M��Q��j�y�ц��KP�L��ozg9�=��`�5o�����w���lh��i�1?��GZ(�4s����Y!�bH��~�oez����8I�6��1�D�w�#;���!��"!���|r�q���&���J$���w��΅��L�Y��T�+��I!؎e�;��q��1��2���+���風v�0��N��~��8׉8ǘE�vkQ���h����}���	���nw����<8��ym����@
54dS2��f�T��L#`��v4/WʎU�B����*<D�#;�*T�Q&��V̛��0w��KJ�`��&v
�dFp/兀���.�>�p����)�吰J(f1A�	����^��S�)Uc�X����$c��c}-���
T��!.�� �=�.Q@h�%�)#L�&r��?�$-��oʆ;���P�@&{3S�6Qe�
��u�:W�ĕ�����-��H�������k���[��1U
�R(�$|$UY��Q�k �]Զ�W�ր^+�W�w�?��Z��z j	��o��=^u��J��Y��J�U�yΜ5w��vn:���٣�9O���4Jt�v�P��s@vF�V�e1p#G��7�WUZ[���E��Wi����x��+����f,Q>4xn�u�f�h覇�!����ٲ v �m��"��Ch>�o�RO4H�      H   �  x�uR�N�@<�~�51�K�K�O�1bB� ����ĳ���Z~�?rf[�D=�nߛ7�v����)���AW8����;F^%C5��|Ɂg9I������D��{�V"������7�F���r8|�"J�.�T.�H#`T=�t!W�1]H�������w�PO1M^�V3����k';PL�'%�]�RxL�3h͉���㑿wH��pd`!�*}/m:ޛ�ۻ+���{���Ǘ.��\]r��Q��t�v��v$8�+g���h��pA�v�vh��Jpp{�b���:c��L�_���>h/���M�_��� <�8�}�d�J�*� � E~̫S��������^z�xA�RF�X�޾���2����'�P�7���={߲�~��@     