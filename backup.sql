PGDMP  "    )                }           ontu_phd    17.2    17.2 C    @           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            A           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            B           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            C           1262    18003    ontu_phd    DATABASE     |   CREATE DATABASE ontu_phd WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
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
       public               postgres    false    217            D           0    0    applydocuments_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.applydocuments_id_seq OWNED BY public.applydocuments.id;
          public               postgres    false    218            �            1259    18010 	   documents    TABLE     �   CREATE TABLE public.documents (
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
       public               postgres    false    219            E           0    0    documents_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.documents_id_seq OWNED BY public.documents.id;
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
       public               postgres    false    233            F           0    0    employees_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.employees_id_seq OWNED BY public.employees.id;
          public               postgres    false    232            �            1259    18016    jobs    TABLE     �   CREATE TABLE public.jobs (
    id integer NOT NULL,
    code character varying(20) NOT NULL,
    title character varying(100) NOT NULL
);
    DROP TABLE public.jobs;
       public         heap r       postgres    false            �            1259    18019    jobs_id_seq    SEQUENCE     �   CREATE SEQUENCE public.jobs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public.jobs_id_seq;
       public               postgres    false    221            G           0    0    jobs_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.jobs_id_seq OWNED BY public.jobs.id;
          public               postgres    false    222            �            1259    18020    news    TABLE       CREATE TABLE public.news (
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
       public               postgres    false    223            H           0    0    news_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.news_id_seq OWNED BY public.news.id;
          public               postgres    false    224            �            1259    18026    programcomponents    TABLE     i  CREATE TABLE public.programcomponents (
    id integer NOT NULL,
    programid integer,
    componenttype character varying(20) NOT NULL,
    componentname character varying(100) NOT NULL,
    componentcredits integer,
    componenthours integer,
    controlform jsonb
);
ALTER TABLE ONLY public.programcomponents ALTER COLUMN controlform SET STORAGE EXTERNAL;
 %   DROP TABLE public.programcomponents;
       public         heap r       postgres    false            �            1259    18031    programcomponents_id_seq    SEQUENCE     �   CREATE SEQUENCE public.programcomponents_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public.programcomponents_id_seq;
       public               postgres    false    225            I           0    0    programcomponents_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.programcomponents_id_seq OWNED BY public.programcomponents.id;
          public               postgres    false    226            �            1259    18032    programjobs    TABLE     `   CREATE TABLE public.programjobs (
    programid integer NOT NULL,
    jobid integer NOT NULL
);
    DROP TABLE public.programjobs;
       public         heap r       postgres    false            �            1259    18035    programs    TABLE     �  CREATE TABLE public.programs (
    id integer NOT NULL,
    name text NOT NULL,
    form jsonb,
    years integer,
    credits integer,
    sum numeric(10,2),
    costs jsonb,
    programcharacteristics jsonb,
    programcompetence jsonb,
    programresults jsonb,
    linkfaculty character varying(255),
    linkfile character varying(255),
    fieldofstudy jsonb,
    speciality jsonb,
    name_eng text,
    degree text,
    purpose text
);
    DROP TABLE public.programs;
       public         heap r       postgres    false            �            1259    18040    programs_id_seq    SEQUENCE     �   CREATE SEQUENCE public.programs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.programs_id_seq;
       public               postgres    false    228            J           0    0    programs_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.programs_id_seq OWNED BY public.programs.id;
          public               postgres    false    229            �            1259    18041    roadmaps    TABLE     �   CREATE TABLE public.roadmaps (
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
       public               postgres    false    230            K           0    0    roadmaps_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.roadmaps_id_seq OWNED BY public.roadmaps.id;
          public               postgres    false    231            ~           2604    18047    applydocuments id    DEFAULT     v   ALTER TABLE ONLY public.applydocuments ALTER COLUMN id SET DEFAULT nextval('public.applydocuments_id_seq'::regclass);
 @   ALTER TABLE public.applydocuments ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    218    217                       2604    18048    documents id    DEFAULT     l   ALTER TABLE ONLY public.documents ALTER COLUMN id SET DEFAULT nextval('public.documents_id_seq'::regclass);
 ;   ALTER TABLE public.documents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219            �           2604    18105    employees id    DEFAULT     l   ALTER TABLE ONLY public.employees ALTER COLUMN id SET DEFAULT nextval('public.employees_id_seq'::regclass);
 ;   ALTER TABLE public.employees ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    233    232    233            �           2604    18049    jobs id    DEFAULT     b   ALTER TABLE ONLY public.jobs ALTER COLUMN id SET DEFAULT nextval('public.jobs_id_seq'::regclass);
 6   ALTER TABLE public.jobs ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    222    221            �           2604    18050    news id    DEFAULT     b   ALTER TABLE ONLY public.news ALTER COLUMN id SET DEFAULT nextval('public.news_id_seq'::regclass);
 6   ALTER TABLE public.news ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    224    223            �           2604    18051    programcomponents id    DEFAULT     |   ALTER TABLE ONLY public.programcomponents ALTER COLUMN id SET DEFAULT nextval('public.programcomponents_id_seq'::regclass);
 C   ALTER TABLE public.programcomponents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    226    225            �           2604    18052    programs id    DEFAULT     j   ALTER TABLE ONLY public.programs ALTER COLUMN id SET DEFAULT nextval('public.programs_id_seq'::regclass);
 :   ALTER TABLE public.programs ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    229    228            �           2604    18053    roadmaps id    DEFAULT     j   ALTER TABLE ONLY public.roadmaps ALTER COLUMN id SET DEFAULT nextval('public.roadmaps_id_seq'::regclass);
 :   ALTER TABLE public.roadmaps ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    231    230            -          0    18004    applydocuments 
   TABLE DATA           `   COPY public.applydocuments (id, name, description, requirements, originalsrequired) FROM stdin;
    public               postgres    false    217   �M       /          0    18010 	   documents 
   TABLE DATA           D   COPY public.documents (id, programid, name, type, link) FROM stdin;
    public               postgres    false    219   �X       =          0    18102 	   employees 
   TABLE DATA           @   COPY public.employees (id, name, "position", photo) FROM stdin;
    public               postgres    false    233   �\       1          0    18016    jobs 
   TABLE DATA           /   COPY public.jobs (id, code, title) FROM stdin;
    public               postgres    false    221   {]       3          0    18020    news 
   TABLE DATA           e   COPY public.news (id, title, summary, maintag, othertags, date, thumbnail, photos, body) FROM stdin;
    public               postgres    false    223   �^       5          0    18026    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, programid, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    225   sc       7          0    18032    programjobs 
   TABLE DATA           7   COPY public.programjobs (programid, jobid) FROM stdin;
    public               postgres    false    227   pe       8          0    18035    programs 
   TABLE DATA           �   COPY public.programs (id, name, form, years, credits, sum, costs, programcharacteristics, programcompetence, programresults, linkfaculty, linkfile, fieldofstudy, speciality, name_eng, degree, purpose) FROM stdin;
    public               postgres    false    228   �e       :          0    18041    roadmaps 
   TABLE DATA           ]   COPY public.roadmaps (id, type, datastart, dataend, additionaltime, description) FROM stdin;
    public               postgres    false    230   "x       L           0    0    applydocuments_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.applydocuments_id_seq', 1, true);
          public               postgres    false    218            M           0    0    documents_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.documents_id_seq', 11, true);
          public               postgres    false    220            N           0    0    employees_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.employees_id_seq', 3, true);
          public               postgres    false    232            O           0    0    jobs_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.jobs_id_seq', 11, true);
          public               postgres    false    222            P           0    0    news_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.news_id_seq', 8, true);
          public               postgres    false    224            Q           0    0    programcomponents_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.programcomponents_id_seq', 27, true);
          public               postgres    false    226            R           0    0    programs_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.programs_id_seq', 8, true);
          public               postgres    false    229            S           0    0    roadmaps_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.roadmaps_id_seq', 13, true);
          public               postgres    false    231            �           2606    18060 "   applydocuments applydocuments_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.applydocuments
    ADD CONSTRAINT applydocuments_pkey PRIMARY KEY (id);
 L   ALTER TABLE ONLY public.applydocuments DROP CONSTRAINT applydocuments_pkey;
       public                 postgres    false    217            �           2606    18062    documents documents_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.documents
    ADD CONSTRAINT documents_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.documents DROP CONSTRAINT documents_pkey;
       public                 postgres    false    219            �           2606    18109    employees employees_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.employees DROP CONSTRAINT employees_pkey;
       public                 postgres    false    233            �           2606    18064    jobs jobs_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.jobs
    ADD CONSTRAINT jobs_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.jobs DROP CONSTRAINT jobs_pkey;
       public                 postgres    false    221            �           2606    18066    news news_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.news
    ADD CONSTRAINT news_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.news DROP CONSTRAINT news_pkey;
       public                 postgres    false    223            �           2606    18068 (   programcomponents programcomponents_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_pkey PRIMARY KEY (id);
 R   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_pkey;
       public                 postgres    false    225            �           2606    18070    programjobs programjobs_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_pkey PRIMARY KEY (programid, jobid);
 F   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_pkey;
       public                 postgres    false    227    227            �           2606    18072    programs programs_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.programs
    ADD CONSTRAINT programs_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.programs DROP CONSTRAINT programs_pkey;
       public                 postgres    false    228            �           2606    18074    roadmaps roadmaps_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.roadmaps
    ADD CONSTRAINT roadmaps_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.roadmaps DROP CONSTRAINT roadmaps_pkey;
       public                 postgres    false    230            �           2606    18075 "   documents documents_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.documents
    ADD CONSTRAINT documents_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id) ON DELETE CASCADE;
 L   ALTER TABLE ONLY public.documents DROP CONSTRAINT documents_programid_fkey;
       public               postgres    false    4755    228    219            �           2606    18080 2   programcomponents programcomponents_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id);
 \   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_programid_fkey;
       public               postgres    false    225    4755    228            �           2606    18085 "   programjobs programjobs_jobid_fkey    FK CONSTRAINT     ~   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_jobid_fkey FOREIGN KEY (jobid) REFERENCES public.jobs(id);
 L   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_jobid_fkey;
       public               postgres    false    4747    221    227            �           2606    18090 &   programjobs programjobs_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id);
 P   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_programid_fkey;
       public               postgres    false    227    4755    228            -   D  x��Z[o�~�~�BO"@*�b�i�؟��)PE�~+
�"Y	 ��ʒhP_R?��//&M�.�������|3���=g))NbK�^Ι�73������/>�ɞf�(K�q6���g�%y?�fY��*�fi��;y7��_K���x⊮v�_?�;�H���v��}lBwW��~�G&�?�{Q�·�!ɇ�Z�_Ӌ��~tH�!�C;oX��u����9�ٗgH�lٌ�>�I+�'�X�zB+b�'t}-+������1��f�J��Y��q�@��<��v=hM���cQ��O�f�a[�J"���&]Z�����}ڇ�n�mc��]뱩&�6]y���Z����������W�({Mo����P|N[uʏN\�cZ�ˮ ���+zt!���)-ě�P��I��ٔ��@i��~��
[�zp��"d�]~���7���p9l�0R}�����v(��^���ِ]<�N��@l��+�E��QR�X�����V�r$e3��F6�%�F�f�%l:�����ˀ��Q@V\0�;��'�a���g�v>����G�����lD��lʽf���|��G_>~���ɁK��߷��:��H=�A��!e� I�Z����̘!�#���I���,���y5#Xϱ�Zb6AHu���A���F�҈u=��S�M��T7�~�0��]I��H{���P�rPZ�(����~]���%)Φ�@�}F����˽�5#�!�T�뾎hi��A�G�U$3����fk,#��-j@����{��b������T�)�kŪjA��ف���̞ �prr/SI�0��:�"A^�ի���"��ǹvȨ���ggn��^!/�#�?ƷM�������%m��]���I�W��Pt�fk�u��bP�)�9���R�VA�2��H�)�b�!�L�쨉���1����6]��3�T�VIƭ�L�[�{08R*�>��	�2���5���J���`-��
 �� 4!F߷�^X��vD��	�ZN��͢H��dC"�O`\^z_��舍����\��l���d	t����܉\3���	���JQcGP�FFI�ޤ���V�3T9y/d��֩s��9G\E����`t�|�jDo��B�����{gX�
KqI����gzDo �β�K�4*W:�3SX8F��kA&'4}c�� ���Q8�AD�"�7�h����h�W@������6���A8m@q-`��Ɏ(c7��MZ�l������]8���5��Q�A�P5�i�3h��I^ذ�D������A�5T}�ō@U��u�%栧Vm�5�C� ����g*v%�*�"	GG�M!W�߾i6#p��Atx�����ֽ�����S�M�tɴ���DxN�[z"��B�]O�z~������9�p��P	�'�b�u�t�x;F.�`N�h5�@�u�~���,|%�҂p^[��
�����ZB���B�t6�{
w����i;ֲ_�*��H��(!�҇Α���>(Hګ!?�K~���9n��V��/�:��P��7u��l�
����`�ra�[k��F�K/���Ԁ]*ُz���k�dj��p�N��Mj�m;�>���#-+�[�h��}���e`1���R����LHpگ�c���F*Pm�I�p�oۑʕ�}�F��T�R^G�)��Gp,#,֓_����tFN�	�xIr1�%�ŶN���� U��l��������x��k��j��Z���/���.�y�T����m���u�+J������
t���1�4�Gd&T�Y�SgL�ԛ,ٍ�fȎ��F�#��XIA��!D{g�F
�	X'�Upr�n���rE�{���)mky��:s�:�O��qV0z���u�;JX�8��Hz���!��r_ۏz�h�8��[���TǄ�S�j�j��Tio�X͏����+Is'2�`��#�E��<OQ|i� �\1C��������V���6��I�l��~��Gj��Z��T��A���+=��ܟ�������+���O_����MP��}mت��X,;ה�o({��+ʹ���CH�(�	:�J�/=p�VfL.,�$[������M1��Z�fbxZ�� i�"�\;��'�yy�Y5�7�o4#3Ԇ�����=�=���w,<ĳՃ]Cs��hG�Vrȏ:��#]o�\���Q��
DՌ���qM=����;,���GE���D9��L��� ��':><��}@�J��m�\u�A�/�� 0�;�1{d6�y�z�����	K�i3U"�4~7�S�n=6�z:�!ȣu2�@�c�p����?����?��Hh�2f� Dwl>ܝ�]s6���ΐ��"����9�r`��wg-�*N��g>c�^����ۜ���	��{��3̓�t0�s,�OK���NF�C�;�,n%��)f�����SOo<��O�Jх��fnE�Z'�cf�C�|�����p�.����4��k[�mf���Q�jf,ă2���Ϳ��i��t ]�5�c��~�an&��gV^ޡ�v(\$ɱ������m�(e']0���t��G;�7.�-���坣8��BH�b�O0�=�2op��"<p,��Bcxirl��r���$T��®I�N��f���|�Cw9�J|K<�2�{4SJNRK��S��1ه	�Bk�a�e���qCx%�{�+eF@y����Q���#�Yy��<6�c�9��!Ǩ'ŉ�z�T�Sy�^tՂ��3�m^0��Ο��Y7� �m��;�(��]yQ��/���Y�83��3�_���5�o<�:�J'�K�и;�u7<7:�������-�v�p����1�`       /   �  x��VMo7=[�b�)Е�_�-@z��5��`��R��rAR2�S%�0Ѓsj�@���C�����o��*9M!)��^�̒o�<�p}��ƚ�~��7��_���0�o���4��ȏ��I�U�#x�T{x;���!SY�tٷ��MG8Փkm�J���Ѕ��e֭wE#��E�E�(�OU�ԍ�螲JI�S��ȓR[�2"�
'��7~��^f���;`��e$'� �3x��w�@��጑W{~��!�r���s��"£O�Qէ�xi�j)���8����m��6/�����cU:~��n�x]ظ��h�JE�F�N�2~�,śm�k�����ŝ� ru �?���RH�OAe: 1�r>�{�ψ��(D�4�8n�����d���: ��9��WdS�RQ�e:�Nhd�s�� �,�c�*��&�0wsg�LY+�{�_�A�"sSRe�D��j\b'��LK���f�\����Zmib|��n��|�A�����}R�����>�@GDӍ�g�l�����j�qǦ̪� ��)2�#,4[���v�gy�ms���uGP�H<n�roA��d��U;����i�����ɠ`��Jcd!��f"m.�Toc�V&�|���� �0��_ �UP��/�������[�E|	:zO��v�������G�1�$F��%�w�W��rج�2��跥0A���Q�8.�(��\>��s��w�}?��v�M�G��a�?���Z��Pv���[	~Z�C��UB(�oԺ1�f5᪏��	��v%(��:�x�P���
'MO�m�Ѹu��6	M/a�6qey�xN݃H�WTl���1Nr�A��"�'��9�@�J0	\�l��U�ݤ�{�1=`H/P�CR�J�>
� �$s��L?�e_�~������D��N��X]�������^���MZ�      =   �   x�u�M
�0���)r���g��s#X���Bݸ�]	.��D�U�ԟ�W����"�./��7o<��^���j�3��p�[<
�?��$ɼ���tQx�-$��,
Sw�D�h������M��2v�Ʉ�v���^�$d�1\�@0sd�cKeg�oe'r+4d���t����%�V��z�V鼓X���9�O���      1   D  x�uR�N�@}f�bK���˿�1-/������i�P�_��#�,����e�3sf0�	�<0=��OA���������Zj�Q��@+��r�cK5'dy	^����r��3��|r��WT�Z2�X���pl����Rk9;�;�����*˽�b;Ȗx��h���S׫s�#������ ��5㼤�BA���]X�ZdxB�	���^0��
�.q6�^xa�C��\9!�K]������ ��P^Z��o�!�)��P�̽0��x|�\�t�=J }t�d���4Z��d�{ j�}e�Fq���1�״��|(���-�%      3   �  x����r�Tǯ��8ε�X���\r���S&�In��21��d(�u:\@q��ʎ��7�{$YR$��Y:g����vW'�E�Xm)�9�����P��o03��6��q��5��z@�L�(P��k}�Y �n]�F�;�܌�����>��w<�`t�����h�+�{u�'�,a����K5��ݷ���rM�mY�}������>��h�����������ޗ��a�B������KW�x�sߢ���ӈ�E.��MMhJc\Gr�Ua��.0x�s[�P�^�3���S	1��Q=�O$�X��h�a��
��~�5Cڙ��C�$Hk��_���EӿF����Ɉ������=�F�O/���ݥ�E�f�.w��8�'//����=N�	��/�����x�"�@�ʡ{�W=�I�?M�|�
��'���9E�	�)=��>�@Q���6�ٖ/ْ�ȯ��x�M��(��$vv��f�K�t�`��Ʋ�����Qͥ&o���s�BM��,��4�nV�uc�o�ʏ,J?����W&~�&%S}O�� ��$���^F�YCDپ��/�+.=x�6�׹������;ݫY�'ojx¢�(��ϰ�ɀ�{1@c�oI��N��9��/㛠�U`b��q7�m�r�g$�	�8��kq�"g���B��-s�-�_��WS������0�A�Z!5�3�o��-ω(�b����_K��W��s�/��/���id�-��,�i[�]ĥZ��\��cK8��9��V�X��~W�{*vF� d�R�[�r���0���A��>AS�`��I��m+n�l�Dzb1�5o٢�JU�z�t���Y/r&�مi^vM�y��	���4�a[X��Y��v�X'a�..nd"X�%Ǎ��B���x2n��)z���G��0r�avB	}h��v��H�z(�TTsȄ;�O�וqT��,V�W��쎣D�it�puW�WǠ�T�u\Q�q�[r��/�TH@y�p�0Qܠ�(�b��� �>/���XNn�|���N
�ka4���$��-x;��N�]z��E��S�`�X`������a�
�n9��Aҭ��$�FQ�Ga Ҁֻ����>�c:�A�����_�녿�����%�8�E���ٶ�?�Q��      5   �  x��T�N�P]�~E�Z	e�+t�JYĠA�I�ILd%qc\W�Z��_���<��%�Dq�оw�,uԨ_՚�SEO<�%�����Mz��yd��x�2��Rǉ-plӊ�e#�#�G�� ���i��ȗ�l�fr��8`"��<�P�WVQh�r/ij�%�,{fyQ�L�����H��M+�]d���M��U��ѫ@�@eUz��l��Ydq+4�����=���os��S	��8���gX�f�Fć�J��^�G��zں	p�5����؊��"��х�.~�(�d1#QX��uǗ|+E�)�G�܇�6�&[�D��6���z���klxw�okc�KQ�����Nw��O������7=d��2�F������8G��AGv#V��������YC�ݡ�/�h�Í�дX)!�.�،��F:z�Z�ݶg`���%K:�?Vk����!�����Y�펒���Q��(U��$i��l      7   3   x�Ĺ  �:�Ðva�9p
�FЖlq	I)9r�n$�ˮ�ӽ�9L�      8      x��\[o�~��@/����v.OI��n�:�Ej 4���P\�\�u�(K6 H�(�&NҦ}�(Q�xY��eBI�efvvw���hFp-�e.���3�٩����y�%��g�7��zU��
�?����3��t�7�A��à�����M�N]�4u��%�/{���GW��oq��5���%um�
_�7���e'7���_
K��t�m�M�!���ZИΈ������=�v'8���V���ᚠ�MX�j����]t��Ȩ0b6�2��#�;ޢ;W	����>l����*U�ru�)y��u�hu��������^y��W+������6\Wp�w��W-;��Gƶ��*�,��~�4����o��Е�{�A��8�\������xN)��{A#8�?���0�����6���@�ș�|f���,;�B.1��8<8�gx�'�U�rl��`�4�
�+��4���{��-³hs`O���u�xj��+�]�Xr����բ�h�X(}�ͻKћ����t�-w�Vx�]nɉZ<�3nM?6�<k}� &0e
���~��7�_\7�~�۷oO-/�M?�[����_E��+����s����àO@f>ؗRD�Å̳`N�e�~��������c`����`3�������K���(zeg�17�a�!z������Sq5�Qț�K�צ.�^��p���K����^��q�ƥ�¥�ĥ��T�44Q���.�i���ux��:�u�V���'�x�g����,�Cxo��؀���vƄ�P!pG �� ���}��&�8T�(P4��P·�'=���Ӷ�_ q��co���n�T|M�!�!0m�|j�C�l	�h��k�� ���>ɮ�a@��9 ��\9�,�F��d�\.L�O�dL >��"�>��x��w��ky�����܂g:(�Xcc9F�J���:���*�7ѧ�y:���юPP��u{�
�\����99�T�ǰ�v��yuM�)-�7Y9h�5��:9zݪQ��#i��Z����==4�:�6y9α�F��S6U���H;"Ɇ�����e���wJB^28�y։*��i�M���~»�"㡍D�5&�`AW��G��8#�Ϣ�� \�y|�/q�Įr*��I��s@��A^0�c����x(E�ءiྖ�9-Jx ) ɰ���gת&S�o���H
.D	��:�7��e�"����ϰB�����49#B2'�g�C=锼�~��@�nQ�|BR'f���st�����Z�87Cm�����@�6� �D��5M��&���A}� �'I�۳�J�5��‘���M����`$�U���9��>E���V��`]�рx�MbT��t�kHG�k;F1�'�@����i9�w��;��Z%5$��Փ���JOY�=�#��w��r��H�D�D�POV�B�c!n��O#�a8ɾa���b=h'���'ݮ�a� �}ø�-�dO��QX��S�>��Ŷ�lӥ}In6q\N���!239A'ᬼ��P��m7+�璢$�l��8@���dQ�uI�	Ւd�8J5��S�GrB���GL�)]E�s�ZQ����΁��m��ЈsF:)�!Z$��UsB1 3�=���zȿ��Q	sd� �x\٘��X�+r7!����J�B�R��/�7�>�U�EU��
4O�+P�^���1� ԸX60�X%"���xp�cI��2q�.�f�X������4'M�R��"KO��K
2^V���6p1f�VB�uE*c�ã�i��$�D"_����2*X�퍟���F��BAFHgj)S��%��ro��H'4g��d& �b*0-a)x�
����1V@��{��閎�IJt����p3K��;�B�_��o�e�W��s���uS�)�g��L�e�m)�g�}���g�m��̅�P%ZRu.��P���L���Kk�E�8)���߫��ٶdp8l�0\e�>mZŮ6�b�"2;!��I�ֹ��AWCV�Z�� i��z]]�W�C��H$���C�5��vz��ҹyR�H��cx+��P"Ӡ���^���S'�f�M"���W��P��M+�J,ׅV�X�W�0�t*Le��@�WL���^�)�ZVV]�3��?�
S=�:��=>MBTl۔<�z%�Io�.�,+?h|	hU�,�'�����Z�3��yD�|��25����#eslU�Ir3�m,>��ӗ�� Gd��~���Tf�_qv�U92��2�
n˧��Qu��N_y�*�u�x�!0oZ�$�1U?�h�Rlo�$ȗ���D��-V�+��E�:e5�b�Ap�kg<!3�&K�ՆH��V�"��ʕ	�Hd��Dr7�!��UzF23j�FuF P���ٞ���ף
ȵ`#fv��췦p,Ո�8f�rOD^4h�z[����z�6�$�be�*�Au��@�Hi�V/��q�J��
���},��e�=]�����4��=�GJ4S!.*c��xQ��p���A�*k�Q���t�6�h�t�*�S�yѼDb���IJ�@���X��c�F�+K��j� %.�鮊�S^hOh�46���r�L��qP�߭8�\�^#��V�U4b!`�α�nf)B�ZQ���1Il���\v:���Yޛ�����3�{��1ʴ��!SM����~��ď����뢞�8-u��J���`-���0A`�+^&���i�ʬN�n^�Y^g����I))��{2�`����ViMR4K	�#<<#���Dk��*����T
�Kb�o�|{re��.��G���>S7��L?SM	�6����e1?>luU)��X�~Mfn�P[�%j�叔(%���٤�aw�l�Жqa�!f#�ۑ�ʰ�C���Z�BC��+G>M�<�JG QmxW��S���Ǧx0n�J2
Ѭ2	i�-U���A�N'�D��QH"-�{�T*aH�~]��6��i�?�Ɍ �Wu�f�*,��[��&:m�ʟ!MK���=���I[���ҍ"�ꡬ#�2�	f\H�^*b��a�S�-Q�S��%9MB��������h=���V�8:����OLg��s��L��G��m���V�Y�_��ӆu:�Ԓ\��F��^�?��'V��5�D�ai>��P4���m�Y���e�z�L���7�%��Q*=�ԉ��bG1�^[~�I�N���,�Q1��nK`s���ċ�e�	s��C9�캱�ʰ.�Pw�m��$qQ��2Nn��&��6�-r�8��TI(��")j48�U���
����%������[�����b�#M�"9�^~�P�����=�f=z�Q2b�c1�\@L�24%�f'JZ<�Kik�8Mέ�f�A �򜩅~i^�%b�*�|R&N��gIW��5_�������G�$&��Դ`oj�'y:U$N��흽���Tj�7V�WsR�!?gC�zվ,���e�!�c�2��t`���)�N��hr�9Z�	hʣ��@W5R�z��%�G��\���d�+U���ISR���Px�0dBFQ� �^�	'`V�Jp���b�m�V8G�Uj��5��i-�������jt_&�M(!�*�$Ў��V�����7�7���^��Y�E��)��3���DN�Y������hv�wزݱ��v��<Z���v��T 4dn�������z �Ƀ�# ?���Ӓ�FVR��I30[ClC;t��ZC���#>cT����
�I ����Y[�χ�O2ѓX!�Hp�p����m>`˱n��P�*� =����I�jbD�v��Lm��{�#���dU��`J6�6���C����y��\�~2͒e����f0��`����A��Rɠ�S�A�%���fih������qeM̲��ol
?Z݌���-�jWm�N��<�:����5�7����]��J1ztuP�Ui{"�o��Y�Ui_~D� co����ZuS@M������§m��<�!� �br�x������~��L)��l��e R�U�K�D�a;�G��7�$� Z  �v.���}�n��x���WXrl��8�\���ԧ�y��滨���\s�wD�:V����,��J�a�C.���P=�嫑9�*�����ۦ��s�
�/%�uF���Kn�]x$�y�f���H|Pv�y��o��y'��\ψ\iN��sx=r�s*�Y�s�ܚ�*K4M�݅��yG�+�%�ȭ��j����s����|�ɕDљ�D~�Z.J�Z*x�Y���쯝N�]ƻޢ[]X�De�}�q���{?W,��s�J�Z��^Ă��ʎ(��ݹG�~��d�{���
l����a�<W�U�bf��xh>����b�s�Yq��U�B��y­�a4��\qy1��J�bug*����Z߲[,��:���n���V+�~b�ɕ�S��/�"���Yt�sN�"�?/]X���B��&,w��,斖`�K�Rn���Hc��|х��f����W'��O��?��ɷ������/��������_�����'�0A�	"����|&H�a���o߹;��	&L0�e��曷��0��\� �&��2 ����bÃ`7��}��y��&�0A��~��oo�y�L���s���R��      :     x��VKn�0]����V�q�d�,�k�`Ǳ�F��jZ�����W�(oF�%r�fS @H������Цݸ����{~�����i��?�A;{M����4��RZb��#M��Q��j�y�ц��KP�L��ozg9�=��`�5o�����w���lh��i�1?��GZ(�4s����Y!�bH��~�oez����8I�6��1�D�w�#;���!��"!���|r�q���&���J$���w��΅��L�Y��T�+��I!؎e�;��q��1��2���+���風v�0��N��~��8׉8ǘE�vkQ���h����}���	���nw����<8��ym����@
54dS2��f�T��L#`��v4/WʎU�B����*<D�#;�*T�Q&��V̛��0w��KJ�`��&v
�dFp/兀���.�>�p����)�吰J(f1A�	����^��S�)Uc�X����$c��c}-���
T��!.�� �=�.Q@h�%�)#L�&r��?�$-��oʆ;���P�@&{3S�6Qe�
��u�:W�ĕ�����-��H�������k���[��1U
�R(�$|$UY��Q�k �]Զ�W�ր^+�W�w�?��Z��z j	��o��=^u��J��Y��J�U�yΜ5w��vn:���٣�9O���4Jt�v�P��s@vF�V�e1p#G��7�WUZ[���E��Wi����x��+����f,Q>4xn�u�f�h覇�!����ٲ v �m��"��Ch>�o�RO4H�     