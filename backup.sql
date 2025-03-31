PGDMP  -    4                }           ontu_phd    17.2    17.2 <    6           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
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
    name_eng text
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
    public               postgres    false    227   F       -          0    17857 	   documents 
   TABLE DATA           D   COPY public.documents (id, programid, name, type, link) FROM stdin;
    public               postgres    false    225   `Q       (          0    17806    jobs 
   TABLE DATA           /   COPY public.jobs (id, code, title) FROM stdin;
    public               postgres    false    220    U       3          0    17993    news 
   TABLE DATA           e   COPY public.news (id, title, summary, maintag, othertags, date, thumbnail, photos, body) FROM stdin;
    public               postgres    false    231   aV       +          0    17828    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, programid, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    223   [       )          0    17812    programjobs 
   TABLE DATA           7   COPY public.programjobs (programid, jobid) FROM stdin;
    public               postgres    false    221   �\       &          0    17797    programs 
   TABLE DATA           �   COPY public.programs (id, name, form, years, credits, sum, costs, programcharacteristics, programcompetence, programresults, linkfaculty, linkfile, fieldofstudy, speciality, name_eng) FROM stdin;
    public               postgres    false    218   ]       1          0    17974    roadmaps 
   TABLE DATA           ]   COPY public.roadmaps (id, type, datastart, dataend, additionaltime, description) FROM stdin;
    public               postgres    false    229   dl       A           0    0    applydocuments_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.applydocuments_id_seq', 1, true);
          public               postgres    false    226            B           0    0    documents_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.documents_id_seq', 11, true);
          public               postgres    false    224            C           0    0    jobs_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.jobs_id_seq', 10, true);
          public               postgres    false    219            D           0    0    news_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.news_id_seq', 8, true);
          public               postgres    false    230            E           0    0    programcomponents_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.programcomponents_id_seq', 22, true);
          public               postgres    false    222            F           0    0    programs_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.programs_id_seq', 2, true);
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
� �$s��L?�e_�~������D��N��X]�������^���MZ�      (   1  x�uQ[N�0��O��F��N��.�M$����B0jH�
�7b�1A�gwfvv�D&1���hC�Z��JM=��ĳ�-u��J&�f��r�����
�J-G��aVϠZ�-�-?�]Sk��'b�;�ox�����b�u\�]P��X�c�w_lY�oǙ��=��:�l�;ќO���f|/��P�;=����^�Psƹu1&S��l���2J��	ʵ�}��1�,D,F�9H xʋEh�	aL���2f%ij��װd�w��>�Q��˒�>0�,X��d�{ *cu=WJ� 0�z�      3   �  x����n�F���S�u�b��|�{�)աE�~ �8E��U')�֍R��+���=4����$K�+̾Q�3KRK�d��er9����f�+סId��Д��Gz�\�x2�9�]�rAK���>�`}HsE3�֧�0�V�`d��9�N�lB>�p}�Oz��&]����21/��A��ʲ���>\֘��q�y��x5�Y�իn������v?ۿ���W�����`��}���/�;����3���{��/]��O����^�sҙإ����Hш�t�ϡ|��0zF���gk�(��W����DD��<��	�,�eMC-&V����]2�ͤ ZJ�Z\�H�ys��kL�F08;�_�)zG��Лr��9X9�t6x�l��߇MN84��$gÚ'�	���⡏/��<���ǒ�3�{U��E��^>}ϐ���G>����Kw���&<��a1�g��#�ۘ<_�m�q��/"�k�	,��q7 ������	k���	���ڪ�����3�;Ń��D/���`U׫��`����nD���|,0�&> �_ؔ~,e����P���c�{���L%:L�ē9���?;�)i8P"�4���������F�;AO?5�f�w���	7�a�O�7̿I �k���� ��q�����D�|D#�Q�c!���n��������L�d���I��?A
������8��K�y�8d�!':(-Ref�5��;��t0�2H�o���E���|�f:Ǚ;���@�9&WT��8�.A�G���FѦm�mZ�"���m�aQ��z�t�0��b]s��5E���X��x���F����k3�o���D���>����t�[w��\�
'��)GDxl�6��h"㕲O0h�n���*[B��O�e����E>�����Ŏ}@	k1�� �4����H���*aK棏���<� k;ȁИ�O3=��ʪC$�G�cmdܕ	X�/�\���#��z����p�|pm�].���M0�A���!o)�,�r���at\��H/c���G�O3ˇ]G�qȓ'�n��`<6n+��d���͜�6�m|�Dڎ֏	Ig��L�n��v�`'�`����|,��*��;�e�Ԥ��? ��|�U���ί9��e�𗗅LnqT:���-}~�T*�
vͥ      +   �  x��S�N�P�o��鬆"<�.N�@�A�Ĺ-��D&��q�@����=o�wn�����д����������]�}[���j�$�xz4����8��H�x"����ą�P�j�YU#��#Z" @ rM�����<����_�e�� L*$��*J�.3bB���k	)�����f/�ٟU �d�v�4�!�E&+	���{Xeؙ�d(�����WL3��,rS?ȉ? F���|������0��J�EP�Τ�;�V�c��k�N��į�3��4�5�q�wԐe�E,�3��Bq7-�,Y�H�l�H��%=p��q��aC����k��P9M���f�Zo�|��!�m�lx%�����t7)�D��=鹘�!�t����s����s�����F�8;u�7��u��=���p���@hZ�p�Xl��y#=wu��X�����ryd�7����      )   *   x�3�4�2�4bc 6bS 6bs � bK 64������ ���      &   6  x��[�n�]�_��Jhَ=��x��m�dL�F�-%�hHt �@e�)��&��|@�����|Q�N�}t��e�,��E��>�y������?y�&��Y>��Y{v�Ѕ񬝟Ӆ�����t�7۟ӧ��M�_���Vo��G�z�SK�VF�����x�����hB�S���|KW;�ޟ�tSs�nc䥯�󌞛�K+4<��Ϸ�k�r>�Sss���å��-���/��[�wo����S-y����_觇�?=^z���S_[�2��[�w�6ئ��4�VH�m����uhn��ᡔ��[������gG"��)	��#��{��	�ѧY���	�qB��9{�B�N�_&4֔�J�!�ODP�"=�O�V:tC�D�dѰ���i`���� �4퀾���h^,�}��oÄ7�+�e��z:|��{4��1���V������ [�4��{v ���r�?YȐ���?]e#h|���z�����B�%�d��Hx��ߗe��z䵼g��8����̐��"kLi0�cq�0T�?l��s�}�71�Κ���+g�*��M Z�%�(���U5�оNo)���>6K��Q�m�7{GoG5JB�
v�/��߿���ӝ�����$8Υ�F��;1դ(
�T��A�Β�}�A�����N!�zs��d^�~>�u�
�==����V0Y�]F�5�����4b:Ҋ�Ճr4�/k�P~�-�m qE�倐�~�4V��`�*�}2�|�����`Q�XD���|@���Q��xO���iY�b C�$�V-�n�ݪ�����p�<��7�w�;/���M(A�CN<;#�al�md<�E[�� ��'�%�&�e�.�8����ꔲ	�Rj���#U������1u.�u66�sV���f��El1�9��3����HO�/���JZ�D�h��hٓB����w�"F�S�\�CY�1��d��t���T�d���l�/8T?�����W[]��ضv�b^A��X3�	ݽ�5��;2�NX+|Zㆬ2ҁ��8��=��ך/w�BZ?�fE�2%H�6�ê��=�ipB�FF��3�X��SO�H��|�9�=	�.���f�9�a��1Mr� m��X'�3��)6a����t���-����:�!��N0�U�1�	nG�I�/�hS���8��"b3�4`p���Q-���P#���� �^/�S\e�K��3�VۖD�g;.4�e�Ȃc'� �c�dыj��hhCp�[���.�뼜��f`TI�C��#��(~EC~�P��}5-6,�j&j����V�.f�o��$����Ĳ h�	���W!wK�� 5M��#��*1)��G�T�f(Yo�úX�$�20�'r�.lZ���-)X����I�!b�a��i�wċ�K?���=U�ԦG[5b�G�$�D#�Y�Afʈ�Vr���3�` ���
j�:Sߘ�,q_Luo@|]��
�I�u02!���д����&P ��)�9-�\�懛L-�l�!�
�����ULw�z�B�v4�����(��I��B&E�Iם�L	�Q���+�;6 ��4'ge�v1:*��!��J����}'�؞��^:�Y��Sَa�G���Q�Ŷ49%<m1�T�6mr� яc�2�Bd!�+��mkK���Y�2 j�X�)-Z@�Q�5ڊF�oZ�{ QO��./��桬��VP٠�I�֋9�K�2���iqؔT��j�ʁ�� ����^/7�R�Y{����֋z���^tfb�)"$c!��1%pʕ2�t`�+�@����k,�fb�'�@=�:��<M *N��P�F���q�"`y4�c~����j�Y���ή�w��>|&���}Z�
����v���F�HU��4F�D|J�g��S� �T��~�+%e�ʼ��[F7#S##�x�%��2T7N8� �L��o�t�`މ I�c�?Z!��m�)F��:Q�`t��t���������ؒ�WK���*7�x2�&S*������H(�DWX`b���������g��Q
�ڊ@	(������!���Tk�F��.��ep��(�8Aߺ'@��M�����,�]c3/Q���`��GWq_�Z��E��wAmd�D+gL��e�ٙ��ىX���һ�{D�Y�ʩ����W���=+�����5a��t�6�h�p1*�kɼb]�1��$�/f�$cQ���Ҧ�Ef�ƒ#�@�"e���XA������clA�-�r�Z���<h��~�O�V�sdE�j������16x�	QiE �KH��O��\q8_���YٛeKLW��{���g�hh�	4�������JJ�oI=?���t@~E���/��F����ON�qP�:��e�>�.`�;8�O���0�����]_�Q��!�YEb���D �Zo��M$����� &i���w�+;Ի��1^��~F�.�'����D�v�r�<Z,+������j������.1C�D�4+W�]w�o�0V	��*�L�q�P^y���(X��-���g�4|���Z(]�f���YO)�[M�,d�`�{�0�����aklsP�ӕQi)L
�NfgTa�A?2rࡽ���!�y ��O�qhҢ��"�w�i�P�eZ�W���!�$���}��=���P9(��3�׫�؞��D�T�H����n�aH>C��x�\|��b��l�TU��s�N�������x��Δh=x���~�v��g%~Ǝ/M�x:���
�!�����k��gx?%G�xZ�w�:���l<c��c䬩Q�*�߇���������M����"&�+���SW���p;���W�j�Z�G��s��=�N�mQp5sa��$��U���F����8����xuI�Q���Ҙ$-E=c��0K:����-
_��ACU�]���8��,���YuV�<b-RJ���Ton־]"�,+�s0.�t����,z*-z�F̂�.T�bx%�k�
��-
-^���:�4NG�,���	�޿cP�b$+?��Qa��a��8oߤt	1l9���Z�L�=��H�q!�"�F�p%��TM���x��s{VU6}KtH�٩`F~�� �֙��n��9�O����80b�XR�T*)Za���S�@`��4UJ���FA#)8�%��A��m��J<V�Ssk#w ��P�G0x�1�Q�nA(� ���m��K�s.61���D�[XG ��1R�{�GF��=ק0B�G��Nz�рg�{(��'N�;�K��o��Q�񤢉q�������K`�8r���~�����؝h	���v�VX��ڀ&H�N=q�&���R? ?��  s��s,�b.SPAчf෉ĆNq���D�&���>0�U����o,�V͆-���,J�x*�a� ӹy��c�'r�Vr��Ѐͪ�(]86p݇O�dNw*��lȖ��s��`����hƖZH�?6U�,�\����d���;��6��z���w�A�]�j��7u�ر4N�i�ȜؑN���*Z����~����n[�����]�+=7ȇ��C����~����gS=F6�a�֞`�D�2*���W E#��[�U�2��0��=P�x9y+�����bA��c�2[��E�V�|�K
��=^_{"�.E�V�vUX�� �WE8��l������g�͍�߮�7��67�͵����V=v�w����.�^^o<�����ږ��G��0*maV�n-z�ն(��yO�"w\������L��ؙ��koEҖ��cݦ__��n<o<{�4�&�Z�s}�U�۝Ɠ����Z�Uc�i}���ئ�dm�I��F�	__�o7w֚��[\�u��� g0      1     x��VKn�0]����V�q�d�,�k�`Ǳ�F��jZ�����W�(oF�%r�fS @H������Цݸ����{~�����i��?�A;{M����4��RZb��#M��Q��j�y�ц��KP�L��ozg9�=��`�5o�����w���lh��i�1?��GZ(�4s����Y!�bH��~�oez����8I�6��1�D�w�#;���!��"!���|r�q���&���J$���w��΅��L�Y��T�+��I!؎e�;��q��1��2���+���風v�0��N��~��8׉8ǘE�vkQ���h����}���	���nw����<8��ym����@
54dS2��f�T��L#`��v4/WʎU�B����*<D�#;�*T�Q&��V̛��0w��KJ�`��&v
�dFp/兀���.�>�p����)�吰J(f1A�	����^��S�)Uc�X����$c��c}-���
T��!.�� �=�.Q@h�%�)#L�&r��?�$-��oʆ;���P�@&{3S�6Qe�
��u�:W�ĕ�����-��H�������k���[��1U
�R(�$|$UY��Q�k �]Զ�W�ր^+�W�w�?��Z��z j	��o��=^u��J��Y��J�U�yΜ5w��vn:���٣�9O���4Jt�v�P��s@vF�V�e1p#G��7�WUZ[���E��Wi����x��+����f,Q>4xn�u�f�h覇�!����ٲ v �m��"��Ch>�o�RO4H�     