PGDMP  1                    }           ontu_phd    17.2    17.2 <    6           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            7           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            8           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            9           1262    17788    ontu_phd    DATABASE     |   CREATE DATABASE ontu_phd WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE ontu_phd;
                     postgres    false            �            1259    17871    applydocuments    TABLE     �   CREATE TABLE public.applydocuments (
    id integer NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    requirements jsonb NOT NULL,
    originalsrequired jsonb NOT NULL
);
 "   DROP TABLE public.applydocuments;
       public         heap r       postgres    false            �            1259    17870    applydocuments_id_seq    SEQUENCE     �   CREATE SEQUENCE public.applydocuments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.applydocuments_id_seq;
       public               postgres    false    227            :           0    0    applydocuments_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.applydocuments_id_seq OWNED BY public.applydocuments.id;
          public               postgres    false    226            �            1259    17857 	   documents    TABLE     �   CREATE TABLE public.documents (
    id integer NOT NULL,
    programid integer,
    name text NOT NULL,
    type text NOT NULL,
    link text NOT NULL
);
    DROP TABLE public.documents;
       public         heap r       postgres    false            �            1259    17856    documents_id_seq    SEQUENCE     �   CREATE SEQUENCE public.documents_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.documents_id_seq;
       public               postgres    false    225            ;           0    0    documents_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.documents_id_seq OWNED BY public.documents.id;
          public               postgres    false    224            �            1259    17806    jobs    TABLE     �   CREATE TABLE public.jobs (
    id integer NOT NULL,
    code character varying(20) NOT NULL,
    title character varying(100) NOT NULL
);
    DROP TABLE public.jobs;
       public         heap r       postgres    false            �            1259    17805    jobs_id_seq    SEQUENCE     �   CREATE SEQUENCE public.jobs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public.jobs_id_seq;
       public               postgres    false    220            <           0    0    jobs_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.jobs_id_seq OWNED BY public.jobs.id;
          public               postgres    false    219            �            1259    17993    news    TABLE       CREATE TABLE public.news (
    id integer NOT NULL,
    title text NOT NULL,
    summary text NOT NULL,
    maintag text NOT NULL,
    othertags jsonb NOT NULL,
    date date NOT NULL,
    thumbnail text NOT NULL,
    photos jsonb NOT NULL,
    body jsonb NOT NULL
);
    DROP TABLE public.news;
       public         heap r       postgres    false            �            1259    17992    news_id_seq    SEQUENCE     �   CREATE SEQUENCE public.news_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public.news_id_seq;
       public               postgres    false    231            =           0    0    news_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.news_id_seq OWNED BY public.news.id;
          public               postgres    false    230            �            1259    17828    programcomponents    TABLE     i  CREATE TABLE public.programcomponents (
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
       public         heap r       postgres    false            �            1259    17827    programcomponents_id_seq    SEQUENCE     �   CREATE SEQUENCE public.programcomponents_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public.programcomponents_id_seq;
       public               postgres    false    223            >           0    0    programcomponents_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.programcomponents_id_seq OWNED BY public.programcomponents.id;
          public               postgres    false    222            �            1259    17812    programjobs    TABLE     `   CREATE TABLE public.programjobs (
    programid integer NOT NULL,
    jobid integer NOT NULL
);
    DROP TABLE public.programjobs;
       public         heap r       postgres    false            �            1259    17797    programs    TABLE     �  CREATE TABLE public.programs (
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
    degree text
);
    DROP TABLE public.programs;
       public         heap r       postgres    false            �            1259    17796    programs_id_seq    SEQUENCE     �   CREATE SEQUENCE public.programs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.programs_id_seq;
       public               postgres    false    218            ?           0    0    programs_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.programs_id_seq OWNED BY public.programs.id;
          public               postgres    false    217            �            1259    17974    roadmaps    TABLE     �   CREATE TABLE public.roadmaps (
    id integer NOT NULL,
    type text NOT NULL,
    datastart date NOT NULL,
    dataend date,
    additionaltime text,
    description text NOT NULL
);
    DROP TABLE public.roadmaps;
       public         heap r       postgres    false            �            1259    17973    roadmaps_id_seq    SEQUENCE     �   CREATE SEQUENCE public.roadmaps_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.roadmaps_id_seq;
       public               postgres    false    229            @           0    0    roadmaps_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.roadmaps_id_seq OWNED BY public.roadmaps.id;
          public               postgres    false    228            }           2604    17881    applydocuments id    DEFAULT     v   ALTER TABLE ONLY public.applydocuments ALTER COLUMN id SET DEFAULT nextval('public.applydocuments_id_seq'::regclass);
 @   ALTER TABLE public.applydocuments ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    227    226    227            |           2604    17883    documents id    DEFAULT     l   ALTER TABLE ONLY public.documents ALTER COLUMN id SET DEFAULT nextval('public.documents_id_seq'::regclass);
 ;   ALTER TABLE public.documents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    225    224    225            z           2604    17884    jobs id    DEFAULT     b   ALTER TABLE ONLY public.jobs ALTER COLUMN id SET DEFAULT nextval('public.jobs_id_seq'::regclass);
 6   ALTER TABLE public.jobs ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219    220                       2604    17996    news id    DEFAULT     b   ALTER TABLE ONLY public.news ALTER COLUMN id SET DEFAULT nextval('public.news_id_seq'::regclass);
 6   ALTER TABLE public.news ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    231    230    231            {           2604    17885    programcomponents id    DEFAULT     |   ALTER TABLE ONLY public.programcomponents ALTER COLUMN id SET DEFAULT nextval('public.programcomponents_id_seq'::regclass);
 C   ALTER TABLE public.programcomponents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    223    222    223            y           2604    17886    programs id    DEFAULT     j   ALTER TABLE ONLY public.programs ALTER COLUMN id SET DEFAULT nextval('public.programs_id_seq'::regclass);
 :   ALTER TABLE public.programs ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    217    218    218            ~           2604    17977    roadmaps id    DEFAULT     j   ALTER TABLE ONLY public.roadmaps ALTER COLUMN id SET DEFAULT nextval('public.roadmaps_id_seq'::regclass);
 :   ALTER TABLE public.roadmaps ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    229    228    229            /          0    17871    applydocuments 
   TABLE DATA           `   COPY public.applydocuments (id, name, description, requirements, originalsrequired) FROM stdin;
    public               postgres    false    227   %F       -          0    17857 	   documents 
   TABLE DATA           D   COPY public.documents (id, programid, name, type, link) FROM stdin;
    public               postgres    false    225   yQ       (          0    17806    jobs 
   TABLE DATA           /   COPY public.jobs (id, code, title) FROM stdin;
    public               postgres    false    220   9U       3          0    17993    news 
   TABLE DATA           e   COPY public.news (id, title, summary, maintag, othertags, date, thumbnail, photos, body) FROM stdin;
    public               postgres    false    231   �V       +          0    17828    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, programid, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    223   <[       )          0    17812    programjobs 
   TABLE DATA           7   COPY public.programjobs (programid, jobid) FROM stdin;
    public               postgres    false    221   9]       &          0    17797    programs 
   TABLE DATA           �   COPY public.programs (id, name, form, years, credits, sum, costs, programcharacteristics, programcompetence, programresults, linkfaculty, linkfile, fieldofstudy, speciality, name_eng, degree) FROM stdin;
    public               postgres    false    218   |]       1          0    17974    roadmaps 
   TABLE DATA           ]   COPY public.roadmaps (id, type, datastart, dataend, additionaltime, description) FROM stdin;
    public               postgres    false    229   �n       A           0    0    applydocuments_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.applydocuments_id_seq', 1, true);
          public               postgres    false    226            B           0    0    documents_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.documents_id_seq', 11, true);
          public               postgres    false    224            C           0    0    jobs_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.jobs_id_seq', 11, true);
          public               postgres    false    219            D           0    0    news_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.news_id_seq', 8, true);
          public               postgres    false    230            E           0    0    programcomponents_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.programcomponents_id_seq', 27, true);
          public               postgres    false    222            F           0    0    programs_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.programs_id_seq', 8, true);
          public               postgres    false    217            G           0    0    roadmaps_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.roadmaps_id_seq', 13, true);
          public               postgres    false    228            �           2606    17878 "   applydocuments applydocuments_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.applydocuments
    ADD CONSTRAINT applydocuments_pkey PRIMARY KEY (id);
 L   ALTER TABLE ONLY public.applydocuments DROP CONSTRAINT applydocuments_pkey;
       public                 postgres    false    227            �           2606    17864    documents documents_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.documents
    ADD CONSTRAINT documents_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.documents DROP CONSTRAINT documents_pkey;
       public                 postgres    false    225            �           2606    17811    jobs jobs_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.jobs
    ADD CONSTRAINT jobs_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.jobs DROP CONSTRAINT jobs_pkey;
       public                 postgres    false    220            �           2606    18000    news news_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.news
    ADD CONSTRAINT news_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.news DROP CONSTRAINT news_pkey;
       public                 postgres    false    231            �           2606    17835 (   programcomponents programcomponents_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_pkey PRIMARY KEY (id);
 R   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_pkey;
       public                 postgres    false    223            �           2606    17816    programjobs programjobs_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_pkey PRIMARY KEY (programid, jobid);
 F   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_pkey;
       public                 postgres    false    221    221            �           2606    17804    programs programs_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.programs
    ADD CONSTRAINT programs_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.programs DROP CONSTRAINT programs_pkey;
       public                 postgres    false    218            �           2606    17981    roadmaps roadmaps_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.roadmaps
    ADD CONSTRAINT roadmaps_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.roadmaps DROP CONSTRAINT roadmaps_pkey;
       public                 postgres    false    229            �           2606    17865 "   documents documents_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.documents
    ADD CONSTRAINT documents_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id) ON DELETE CASCADE;
 L   ALTER TABLE ONLY public.documents DROP CONSTRAINT documents_programid_fkey;
       public               postgres    false    4737    225    218            �           2606    17836 2   programcomponents programcomponents_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id);
 \   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_programid_fkey;
       public               postgres    false    218    223    4737            �           2606    17822 "   programjobs programjobs_jobid_fkey    FK CONSTRAINT     ~   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_jobid_fkey FOREIGN KEY (jobid) REFERENCES public.jobs(id);
 L   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_jobid_fkey;
       public               postgres    false    4739    221    220            �           2606    17817 &   programjobs programjobs_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id);
 P   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_programid_fkey;
       public               postgres    false    218    221    4737            /   D  x��Z[o�~�~�BO"@*�b�i�؟��)PE�~+
�"Y	 ��ʒhP_R?��//&M�.�������|3���=g))NbK�^Ι�73������/>�ɞf�(K�q6���g�%y?�fY��*�fi��;y7��_K���x⊮v�_?�;�H���v��}lBwW��~�G&�?�{Q�·�!ɇ�Z�_Ӌ��~tH�!�C;oX��u����9�ٗgH�lٌ�>�I+�'�X�zB+b�'t}-+������1��f�J��Y��q�@��<��v=hM���cQ��O�f�a[�J"���&]Z�����}ڇ�n�mc��]뱩&�6]y���Z����������W�({Mo����P|N[uʏN\�cZ�ˮ ���+zt!���)-ě�P��I��ٔ��@i��~��
[�zp��"d�]~���7���p9l�0R}�����v(��^���ِ]<�N��@l��+�E��QR�X�����V�r$e3��F6�%�F�f�%l:�����ˀ��Q@V\0�;��'�a���g�v>����G�����lD��lʽf���|��G_>~���ɁK��߷��:��H=�A��!e� I�Z����̘!�#���I���,���y5#Xϱ�Zb6AHu���A���F�҈u=��S�M��T7�~�0��]I��H{���P�rPZ�(����~]���%)Φ�@�}F����˽�5#�!�T�뾎hi��A�G�U$3����fk,#��-j@����{��b������T�)�kŪjA��ف���̞ �prr/SI�0��:�"A^�ի���"��ǹvȨ���ggn��^!/�#�?ƷM�������%m��]���I�W��Pt�fk�u��bP�)�9���R�VA�2��H�)�b�!�L�쨉���1����6]��3�T�VIƭ�L�[�{08R*�>��	�2���5���J���`-��
 �� 4!F߷�^X��vD��	�ZN��͢H��dC"�O`\^z_��舍����\��l���d	t����܉\3���	���JQcGP�FFI�ޤ���V�3T9y/d��֩s��9G\E����`t�|�jDo��B�����{gX�
KqI����gzDo �β�K�4*W:�3SX8F��kA&'4}c�� ���Q8�AD�"�7�h����h�W@������6���A8m@q-`��Ɏ(c7��MZ�l������]8���5��Q�A�P5�i�3h��I^ذ�D������A�5T}�ō@U��u�%栧Vm�5�C� ����g*v%�*�"	GG�M!W�߾i6#p��Atx�����ֽ�����S�M�tɴ���DxN�[z"��B�]O�z~������9�p��P	�'�b�u�t�x;F.�`N�h5�@�u�~���,|%�҂p^[��
�����ZB���B�t6�{
w����i;ֲ_�*��H��(!�҇Α���>(Hګ!?�K~���9n��V��/�:��P��7u��l�
����`�ra�[k��F�K/���Ԁ]*ُz���k�dj��p�N��Mj�m;�>���#-+�[�h��}���e`1���R����LHpگ�c���F*Pm�I�p�oۑʕ�}�F��T�R^G�)��Gp,#,֓_����tFN�	�xIr1�%�ŶN���� U��l��������x��k��j��Z���/���.�y�T����m���u�+J������
t���1�4�Gd&T�Y�SgL�ԛ,ٍ�fȎ��F�#��XIA��!D{g�F
�	X'�Upr�n���rE�{���)mky��:s�:�O��qV0z���u�;JX�8��Hz���!��r_ۏz�h�8��[���TǄ�S�j�j��Tio�X͏����+Is'2�`��#�E��<OQ|i� �\1C��������V���6��I�l��~��Gj��Z��T��A���+=��ܟ�������+���O_����MP��}mت��X,;ה�o({��+ʹ���CH�(�	:�J�/=p�VfL.,�$[������M1��Z�fbxZ�� i�"�\;��'�yy�Y5�7�o4#3Ԇ�����=�=���w,<ĳՃ]Cs��hG�Vrȏ:��#]o�\���Q��
DՌ���qM=����;,���GE���D9��L��� ��':><��}@�J��m�\u�A�/�� 0�;�1{d6�y�z�����	K�i3U"�4~7�S�n=6�z:�!ȣu2�@�c�p����?����?��Hh�2f� Dwl>ܝ�]s6���ΐ��"����9�r`��wg-�*N��g>c�^����ۜ���	��{��3̓�t0�s,�OK���NF�C�;�,n%��)f�����SOo<��O�Jх��fnE�Z'�cf�C�|�����p�.����4��k[�mf���Q�jf,ă2���Ϳ��i��t ]�5�c��~�an&��gV^ޡ�v(\$ɱ������m�(e']0���t��G;�7.�-���坣8��BH�b�O0�=�2op��"<p,��Bcxirl��r���$T��®I�N��f���|�Cw9�J|K<�2�{4SJNRK��S��1ه	�Bk�a�e���qCx%�{�+eF@y����Q���#�Yy��<6�c�9��!Ǩ'ŉ�z�T�Sy�^tՂ��3�m^0��Ο��Y7� �m��;�(��]yQ��/���Y�83��3�_���5�o<�:�J'�K�и;�u7<7:�������-�v�p����1�`       -   �  x��VMo7=[�b�)Е�_�-@z��5��`��R��rAR2�S%�0Ѓsj�@���C�����o��*9M!)��^�̒o�<�p}��ƚ�~��7��_���0�o���4��ȏ��I�U�#x�T{x;���!SY�tٷ��MG8Փkm�J���Ѕ��e֭wE#��E�E�(�OU�ԍ�螲JI�S��ȓR[�2"�
'��7~��^f���;`��e$'� �3x��w�@��጑W{~��!�r���s��"£O�Qէ�xi�j)���8����m��6/�����cU:~��n�x]ظ��h�JE�F�N�2~�,śm�k�����ŝ� ru �?���RH�OAe: 1�r>�{�ψ��(D�4�8n�����d���: ��9��WdS�RQ�e:�Nhd�s�� �,�c�*��&�0wsg�LY+�{�_�A�"sSRe�D��j\b'��LK���f�\����Zmib|��n��|�A�����}R�����>�@GDӍ�g�l�����j�qǦ̪� ��)2�#,4[���v�gy�ms���uGP�H<n�roA��d��U;����i�����ɠ`��Jcd!��f"m.�Toc�V&�|���� �0��_ �UP��/�������[�E|	:zO��v�������G�1�$F��%�w�W��rج�2��跥0A���Q�8.�(��\>��s��w�}?��v�M�G��a�?���Z��Pv���[	~Z�C��UB(�oԺ1�f5᪏��	��v%(��:�x�P���
'MO�m�Ѹu��6	M/a�6qey�xN݃H�WTl���1Nr�A��"�'��9�@�J0	\�l��U�ݤ�{�1=`H/P�CR�J�>
� �$s��L?�e_�~������D��N��X]�������^���MZ�      (   D  x�uR�N�@}f�bK���˿�1-/������i�P�_��#�,����e�3sf0�	�<0=��OA���������Zj�Q��@+��r�cK5'dy	^����r��3��|r��WT�Z2�X���pl����Rk9;�;�����*˽�b;Ȗx��h���S׫s�#������ ��5㼤�BA���]X�ZdxB�	���^0��
�.q6�^xa�C��\9!�K]������ ��P^Z��o�!�)��P�̽0��x|�\�t�=J }t�d���4Z��d�{ j�}e�Fq���1�״��|(���-�%      3   �  x����n�F���S�u�b��|�{�)աE�~ �8E��U')�֍R��+���=4����$K�+̾Q�3KRK�d��er9����f�+סId��Д��Gz�\�x2�9�]�rAK���>�`}HsE3�֧�0�V�`d��9�N�lB>�p}�Oz��&]����21/��A��ʲ���>\֘��q�y��x5�Y�իn������v?ۿ���W�����`��}���/�;����3���{��/]��O����^�sҙإ����Hш�t�ϡ|��0zF���gk�(��W����DD��<��	�,�eMC-&V����]2�ͤ ZJ�Z\�H�ys��kL�F08;�_�)zG��Лr��9X9�t6x�l��߇MN84��$gÚ'�	���⡏/��<���ǒ�3�{U��E��^>}ϐ���G>����Kw���&<��a1�g��#�ۘ<_�m�q��/"�k�	,��q7 ������	k���	���ڪ�����3�;Ń��D/���`U׫��`����nD���|,0�&> �_ؔ~,e����P���c�{���L%:L�ē9���?;�)i8P"�4���������F�;AO?5�f�w���	7�a�O�7̿I �k���� ��q�����D�|D#�Q�c!���n��������L�d���I��?A
������8��K�y�8d�!':(-Ref�5��;��t0�2H�o���E���|�f:Ǚ;���@�9&WT��8�.A�G���FѦm�mZ�"���m�aQ��z�t�0��b]s��5E���X��x���F����k3�o���D���>����t�[w��\�
'��)GDxl�6��h"㕲O0h�n���*[B��O�e����E>�����Ŏ}@	k1�� �4����H���*aK棏���<� k;ȁИ�O3=��ʪC$�G�cmdܕ	X�/�\���#��z����p�|pm�].���M0�A���!o)�,�r���at\��H/c���G�O3ˇ]G�qȓ'�n��`<6n+��d���͜�6�m|�Dڎ֏	Ig��L�n��v�`'�`����|,��*��;�e�Ԥ��? ��|�U���ί9��e�𗗅LnqT:���-}~�T*�
vͥ      +   �  x��T�N�P]�~E�Z	e�+t�JYĠA�I�ILd%qc\W�Z��_���<��%�Dq�оw�,uԨ_՚�SEO<�%�����Mz��yd��x�2��Rǉ-plӊ�e#�#�G�� ���i��ȗ�l�fr��8`"��<�P�WVQh�r/ij�%�,{fyQ�L�����H��M+�]d���M��U��ѫ@�@eUz��l��Ydq+4�����=���os��S	��8���gX�f�Fć�J��^�G��zں	p�5����؊��"��х�.~�(�d1#QX��uǗ|+E�)�G�܇�6�&[�D��6���z���klxw�okc�KQ�����Nw��O������7=d��2�F������8G��AGv#V��������YC�ݡ�/�h�Í�дX)!�.�،��F:z�Z�ݶg`���%K:�?Vk����!�����Y�펒���Q��(U��$i��l      )   3   x�Ĺ  �:�Ðva�9p
�FЖlq	I)9r�n$�ˮ�ӽ�9L�      &      x��[[o�~��BO6@˲,9�''ȭM� �K��"ӱK4d��a�DY�A�ť�6�6}�HQ�Dr���	�%��f�ew�R��<�P���^�e.�|3s��X�Ϥ=|��8����a}�хް��Ѕä�$���pe�A?u����p�L�v���`�J6K݊i�]z`�FL=ޤ��~4�����|LW�����ts�nc䱏Ɠ���Kgix��?�/E�I'i���7ƦǦg�.N_~irrbrr�K�Rti�e���S��4��?�{8>�T�5����q�`��j��X!I@��B���&�MB�ѣx�A���h�x��u�p�U�Dʻ��&�o��ѦY��³�&��O��9w=�Н(ُh����C� Ez"��4醊�4ɢa599����6��A6h��F7;�Yx�4�&o�~�F�_�5{������L�K��-�̾�oE0���!�n	���z��@�诜�O�%���&�*���<We�y��*�dF 1�8 ����e���=�Z��[i���mz��b��56h0�cz�0T�?l��s��7��Ϊ�˕��ΰ#T|>�=�@��K`Qv6�3�j>�}u�����{�,��7E9l�5����(	)Nّ�������;C�õ�Ip�}c���'b�QZ�h;�d�%�{+|�v����B������{�z����U����M��в��s�eI��-�Dh5���4H+�WW�h��T��,[�� pE�e@�I�=��M<3N�>C>-��m^p\�8��I=I�F�	�Q��-<���V���,a1�.#ɰV
��g�������bCd.�P��������bJ��aȾgg�P�eOo�}+ Z�Ĳ���y��%�K桞:�l@�C^jС��� u���GH��Aw��Mw���|�Y�v[5#�(����q�2w_��Қ�H!����AdOJ�ۋ�(�8�璀���"&Ҕh�1��*4�qN��G��k�����5��x�MnT��b�k�#֭���$k2��w���cyG��ik�O+n��!#��#+��Vy�z�̊!���o��Q�C�(�l�V��C�;�1���C�7<�",֓N��i�~��r��L�ݡ0�ޠI�#��-�u�=��]�&,�-�`�v��<�q#�TS43�	�9g�m`B]�ۃ�(�^)�@�dCL���L�A���)1���ɨ p�j�I��91A��S&��WY�������%��٦��F�3���I v�"Y���C� �^�V�!����F%��U�D�����bC�H�?�_ѐ�rb������	5}5^g}f�tc�7k�Y0�<1+Zl�je�J�n�q���Ӳ�q0"y|����xx�}%o������8����A ��p� 7i��%)hڔ����Zz>h8F�2�`��2���y1~��V�����h�F�y�����(��5�d6�Q�J��7yF1o����3�����W������=b8ɂ�f&$�e0�XJ�n��s>%�F��B4��05ݲ1>O��"��>��dpǖ�-�����Q�A��B,I�	���L	�Q���+��0+�4����g;���+�B�6V��T0�v�0������U��y�ʶ3^�����.���)�h#�p�5�ش�]�c�!2[!��y趹�������2' j�X�$)5Z@�Y�5ڊ"��7�®B���˲�y^�L��}z+�� �i`�{#��*Ş����´)*"��ĕ��� U���=����4{������jyq�,h�`�I"$b!�HaJ�Ke����WN�\n/���,�fb�"�z�uz�{v���8�v�<C���{�M��E��h W�a��A�qf��8�����\�Ѧ5�����0�+8U��ǆ�0SUr3m>%�3Vv��f�#2fk��+e.�y�W+�nL��X�]\�m��nNUWt�ƋLᯋ�i`�I�c�~Ե6t��޶IP�M̎�D����������㸎MY}�j���c&�d2�^�ڐ����P�k�\�� +���]Q?۫zF>3j@�6#�cj$l/fJ%�	�Z�?�kH�[�8�j�i��o�(���Bo��AH�®7��&�K��U|�0ڊGKy_�����q��xT���1�j@��p�����-L����="�,��l��~/�	�+�n����P���g��t�6�h�t1(�#�y�D1�c�$�/�@��(��of��"�5cɁX	 q�4�vU� ��B{���cla�5�r�R��q����,h����I��*�p>̱Q7!
��"��m��ҟ+L�Sy>+{��ӕ{*x�8F�6��j�
�����8�J2�o�z~4�������R��D�#ؔ��O?9p�k����-k���B�����I#%����ɇ0dy�Wo�J�Ѭ ����R$D��BW�&�^5)�3�IZ���uekz�W��+����h�%�$��ДHݮ���U)�ũ���F��V��	��4s�C�K�P���(嚕�W�O]w�o�0�Ia������Tz��㮢`q��<gh�p�H���#�t
͆�i��|J���ؔ�Bƍ����f��)R�o�j�m
u:�$���I!��pېJ#5�׌xh/����˘�( �S'h:4a��o�Mv�4�?A�VP��z`��$���m�ҽ"�顬1h��3)�\��=�+	����($)Az�JN󐼍~z�x��j�ӥL.T�V�8:a���/Lg����ՙLY�km����v�Y�߰�KӺ:��	jk��Ț��k��gx?�"��q+��py���MB�xzR�gM�R����}h��ɧo-K��(=&�ԡ��bG��P~X��NO�zƫ�Hwζ%8��3��{�ņ�М�١��]7��]�ꆶ�:%�8$mD=W��0K�u�C0�-J�8���$T�jQ��N~�ɝ^��ڳ�8Es�Z$}K9[��a�\�xd	X*��kɏs*
���qO���Y�L��.�������޼�Dy��{�#mMs�)������h������J�\#T>�d*L���O���yk6�������m�wO� �H����`oa�':5ʝ:U�;y=�P��Fo���TC~͆ zձ���2���O�d�?=0��'aJ���M�<�+6N���<4}U ���k�kl�(�<������o�T�2I�0�P#�!wx�1!���� /�	���s��뭘o�|��Α|Ĕ�]���d:@K����¸��F�e\o�yTa'�v����Lo�z�Rzmd�L�7'J���+XΆ ������w��<��	��a��?Z�����T@447�t��מ_��O�Įÿ�%퍬���fෆĆ�p��ZCG��#>ǨB��C ����Y%[�����R�$��9N��1�Fn�[�u�(uج

 ��
��������d��M�l�vDSL����������!��GSp-j�b��eswv�q2���3wŠZ/�d��o���h�:��pB�8}��Tr�`Z��<|S�9�fүn�V�lhw��@\'�v��D�/�Jw�|*%�q`�V�g�	f��fYiPE_~���n��;Xuw)��I'�{:�Ƴ�i[9x��=��>�V��J�ŧ]���Eg���}�T��$.����"Xu����hb����j��W/\�t�z��'s��������|u~��}�\�c�ryz���\�&��o��g��N��CA�.���U
�e�]	>l�+�"wo�s���RspU�	Ǝ�m_yg%j�2�P�s4������ʝʧ�ʭ�����K��*7��U7*���s���"]/E��7��*��|}��X]����ݽ}�̥1�6?&�'_��_�r�M��������ߒП��Ͽ�O�כ������ř�I��̤�F�����Zs�~�y�|�yE���.��bb�U�Y���r�����o$'�k���_�n�����a�ŏ��}1����<��g�Mtd����<�-�y�[/��J4�þ��X�nې�t"y�r�CSr�j�aH��Z�E�6�2   �3��)�M�������r�IO��L�n�a��IJ�?fC��(}�C���_�ߚ����������T�7��ݝ��o(�Ã�� �ժ��@�}��t��������>����W�?���y���^�
w����K��~����"�����τ���WN����O��Epw�e��7+sg.�����˧���"8<;���/�<���׮����˟�������w�����/�\>y���߿I�N7�L~:u�S�?u���w��W_����&Μ9�?,�3      1     x��VKn�0]����V�q�d�,�k�`Ǳ�F��jZ�����W�(oF�%r�fS @H������Цݸ����{~�����i��?�A;{M����4��RZb��#M��Q��j�y�ц��KP�L��ozg9�=��`�5o�����w���lh��i�1?��GZ(�4s����Y!�bH��~�oez����8I�6��1�D�w�#;���!��"!���|r�q���&���J$���w��΅��L�Y��T�+��I!؎e�;��q��1��2���+���風v�0��N��~��8׉8ǘE�vkQ���h����}���	���nw����<8��ym����@
54dS2��f�T��L#`��v4/WʎU�B����*<D�#;�*T�Q&��V̛��0w��KJ�`��&v
�dFp/兀���.�>�p����)�吰J(f1A�	����^��S�)Uc�X����$c��c}-���
T��!.�� �=�.Q@h�%�)#L�&r��?�$-��oʆ;���P�@&{3S�6Qe�
��u�:W�ĕ�����-��H�������k���[��1U
�R(�$|$UY��Q�k �]Զ�W�ր^+�W�w�?��Z��z j	��o��=^u��J��Y��J�U�yΜ5w��vn:���٣�9O���4Jt�v�P��s@vF�V�e1p#G��7�WUZ[���E��Wi����x��+����f,Q>4xn�u�f�h覇�!����ٲ v �m��"��Ch>�o�RO4H�     