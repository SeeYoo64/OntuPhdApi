PGDMP                      }           ontu_phd    17.2    17.2 D    K           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            L           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            M           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            N           1262    18003    ontu_phd    DATABASE     |   CREATE DATABASE ontu_phd WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
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
       public               postgres    false    217            O           0    0    applydocuments_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.applydocuments_id_seq OWNED BY public.applydocuments.id;
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
       public               postgres    false    219            P           0    0    documents_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.documents_id_seq OWNED BY public.documents.id;
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
       public               postgres    false    226            Q           0    0    employees_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.employees_id_seq OWNED BY public.employees.id;
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
       public               postgres    false    230            R           0    0 
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
       public               postgres    false    221            S           0    0    news_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.news_id_seq OWNED BY public.news.id;
          public               postgres    false    222            �            1259    18181    program    TABLE     �  CREATE TABLE public.program (
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
    directions jsonb DEFAULT '[]'::jsonb,
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
       public               postgres    false    232            T           0    0    program_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.program_id_seq OWNED BY public.program.id;
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
       public               postgres    false    234            U           0    0    programcomponents_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.programcomponents_id_seq OWNED BY public.programcomponents.id;
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
       public               postgres    false    223            V           0    0    roadmaps_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.roadmaps_id_seq OWNED BY public.roadmaps.id;
          public               postgres    false    224            �            1259    18119 
   speciality    TABLE     �   CREATE TABLE public.speciality (
    code character varying(50) NOT NULL,
    name text NOT NULL,
    field_code character varying(50) NOT NULL
);
    DROP TABLE public.speciality;
       public         heap r       postgres    false            �           2604    18047    applydocuments id    DEFAULT     v   ALTER TABLE ONLY public.applydocuments ALTER COLUMN id SET DEFAULT nextval('public.applydocuments_id_seq'::regclass);
 @   ALTER TABLE public.applydocuments ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    218    217            �           2604    18048    documents id    DEFAULT     l   ALTER TABLE ONLY public.documents ALTER COLUMN id SET DEFAULT nextval('public.documents_id_seq'::regclass);
 ;   ALTER TABLE public.documents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219            �           2604    18105    employees id    DEFAULT     l   ALTER TABLE ONLY public.employees ALTER COLUMN id SET DEFAULT nextval('public.employees_id_seq'::regclass);
 ;   ALTER TABLE public.employees ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    225    226    226            �           2604    18135    job id    DEFAULT     `   ALTER TABLE ONLY public.job ALTER COLUMN id SET DEFAULT nextval('public.job_id_seq'::regclass);
 5   ALTER TABLE public.job ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    229    230    230            �           2604    18050    news id    DEFAULT     b   ALTER TABLE ONLY public.news ALTER COLUMN id SET DEFAULT nextval('public.news_id_seq'::regclass);
 6   ALTER TABLE public.news ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    222    221            �           2604    18184 
   program id    DEFAULT     h   ALTER TABLE ONLY public.program ALTER COLUMN id SET DEFAULT nextval('public.program_id_seq'::regclass);
 9   ALTER TABLE public.program ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    232    231    232            �           2604    18208    programcomponents id    DEFAULT     |   ALTER TABLE ONLY public.programcomponents ALTER COLUMN id SET DEFAULT nextval('public.programcomponents_id_seq'::regclass);
 C   ALTER TABLE public.programcomponents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    234    233    234            �           2604    18053    roadmaps id    DEFAULT     j   ALTER TABLE ONLY public.roadmaps ALTER COLUMN id SET DEFAULT nextval('public.roadmaps_id_seq'::regclass);
 :   ALTER TABLE public.roadmaps ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    224    223            7          0    18004    applydocuments 
   TABLE DATA           `   COPY public.applydocuments (id, name, description, requirements, originalsrequired) FROM stdin;
    public               postgres    false    217   N       9          0    18010 	   documents 
   TABLE DATA           D   COPY public.documents (id, programid, name, type, link) FROM stdin;
    public               postgres    false    219   "Z       @          0    18102 	   employees 
   TABLE DATA           @   COPY public.employees (id, name, "position", photo) FROM stdin;
    public               postgres    false    226   �]       A          0    18112    field_of_study 
   TABLE DATA           4   COPY public.field_of_study (code, name) FROM stdin;
    public               postgres    false    227   �^       D          0    18132    job 
   TABLE DATA           :   COPY public.job (id, code, title, program_id) FROM stdin;
    public               postgres    false    230   }_       ;          0    18020    news 
   TABLE DATA           e   COPY public.news (id, title, summary, maintag, othertags, date, thumbnail, photos, body) FROM stdin;
    public               postgres    false    221   �`       F          0    18181    program 
   TABLE DATA           �   COPY public.program (id, degree, name, name_code, field_of_study, speciality, form, purpose, years, credits, program_characteristics, program_competence, results, link_faculty, link_file, accredited, directions, objects) FROM stdin;
    public               postgres    false    232   \e       H          0    18205    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, program_id, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    234   Is       =          0    18041    roadmaps 
   TABLE DATA           ]   COPY public.roadmaps (id, type, datastart, dataend, additionaltime, description) FROM stdin;
    public               postgres    false    223   u       B          0    18119 
   speciality 
   TABLE DATA           <   COPY public.speciality (code, name, field_code) FROM stdin;
    public               postgres    false    228   Kx       W           0    0    applydocuments_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.applydocuments_id_seq', 1, true);
          public               postgres    false    218            X           0    0    documents_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.documents_id_seq', 11, true);
          public               postgres    false    220            Y           0    0    employees_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.employees_id_seq', 3, true);
          public               postgres    false    225            Z           0    0 
   job_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.job_id_seq', 10, true);
          public               postgres    false    229            [           0    0    news_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.news_id_seq', 8, true);
          public               postgres    false    222            \           0    0    program_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.program_id_seq', 1, true);
          public               postgres    false    231            ]           0    0    programcomponents_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.programcomponents_id_seq', 11, true);
          public               postgres    false    233            ^           0    0    roadmaps_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.roadmaps_id_seq', 13, true);
          public               postgres    false    224            �           2606    18060 "   applydocuments applydocuments_pkey 
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
       public               postgres    false    4769    232    234            �           2606    18126 %   speciality speciality_field_code_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.speciality
    ADD CONSTRAINT speciality_field_code_fkey FOREIGN KEY (field_code) REFERENCES public.field_of_study(code);
 O   ALTER TABLE ONLY public.speciality DROP CONSTRAINT speciality_field_code_fkey;
       public               postgres    false    4763    227    228            7   �  x��[�n�}��b�'	 ]I6� }l?�@�8@M���(@R����h�2��-F���P��4/C����_ȗt���9s;��\�U��p�}]{�}�ݵ��|��<K�Q2O�I/=���d���d����L�V�|���=y7�S�4��r�%�v�6qK�H�~��yn"�N���%���g��Ei#����?��y�,�_M��#�����If����v${���$ע�-{�
�/��>�?Kb��r}�+�}z�K�
��NY���V�� Ǒ��v{"�TW��規�d�i[�*"���\��G��;Ѧ��b�6�/��`���-W.(cCȻqz�t_~N^N.�Qx*O��:T|([5�����d�\!1c��˭#]D���B�<���My$N�r�L(���.4�pkQ��ɩ�t������x�<^�Q!.;[�)?;��x��/��Bdh�8�!�`��s�Zu�(1D�g�����3	wZ��LJF�Ml�%ZN�ZD��a:��3��6����8Bx7q��i�c��=L��Z��_7>x���7>�6�3�ކ)7j�������g>}���?�C$�,>���%th0N|"�a�BS�i1H�T#5шR:3$������L_$)�nZ�f�@�z�-���s�TS��݊���*F��%J=(t�`�.h�F4���"ũ�({��HY*!<
_0��ש�tX
q�h�\�������jQ�!�M�뾉di^� �#EE�,h�AF�b�P���sv�3�Y,��u�)�Ҽ��j$���M��+���Hc� ��\�K_��v�.���X��_��S���D�d��;ȣ� .o2�o�[e�e�/cٚ*�Y�`�s�h��%���߬̃|��,�G���=����f�D�T@q,��d����Yj���}=���RI�b n5g�uѰ�� �#��b�\)E��Y�H�W��$k�?KL�t�j� �	0����̂�fA¼�Kmس��mF���FDNѸXzS�&�h��QluoTF/;L05������B�$��Hc�iB{~uŬ����J����{��V��7�����I�BFYX���y�*�ݜ�&��gP#z�o�m��5�p����)����ީ"�/�E��^/jT�tFg���� ߶�,N�U�ax�(ħga���(o���n�g��8at��g�P�铊X��iA����8td�� P��hq�b��U�\�V�.�A�������$[�ʴ74`a�8IU�{��Gz>�Eހ5�|�̶U����Ls�zZ�^3ϑ(�Hi��3%���_Q������ݚ�s�o��;۵��tz+ھ�A���n��v��݊S���銴�%���r��[{"��J�]��zm�:����!�p`��uE��4D���Y��2��(�AL�Y5F��l�v��2�!|	EeA:����a���d�N-%m_	��,���=E~�ZE}�vLu1\���i��(%�ڇ�d�y}X��WWB~��W�Z2�~p���A���d�LG�'����)J��L;����&M�~�5yzA��S�T�Oz�ϭ[�d��� C�x�3�N�o�ܳ�����m���_}�>��h�8�1���|F�₽ס�%r���Q�qg&��NS6A�o��ʹ�#�������/��hR�7�V�XGX�Y���w�Q�ЄH��܌h	��֩�_��&���%��t ��p�QV ߖ��t-�[�%������:rڅ���'*���.O��������G��
t���1�4�#:S���3.A��,��[5C�f^��MG���M�M���9DY���� V1�`��tn;�]��0ƶ�N\;���L��233F�q@�[K�������y��g������!=���he�nG�"�@��.}s�1*7���i��L��x���%c%m�!c
�S	�4rWU*��,�O\��34]�5~6��\���A��̹8���M������Vy<��z�+JO���Sj����%��Z}ez���WV�����P���ڰ����bѹ�D(c�s�ߢ�XB� Y�#)�f�#X5��(,���-��
�9K-��;b֦!�������o�|"�v�G�����g�|��#��}���6�^>0�~V�{�;k�����SA�[>�u4G�LHD�淂C��)��V���u"ʐ)��%��-G�PST?�q��&����Q���:QD�*Ӹ�$(��n�J�(��=eG||Ho;���aXה.��|f���I�[�v=�m�j�ں�؇�+�W�T¥��t���O ��(d\=���i��?�[hC7NPV�u��]r�n
e��? ��K�<U���ܛ���)\u�O|�r� 7)�9Hy����-a���ÖW�ɝff.�~��$B�Yap_�霽c{��M�r6��@��qn\��AeicNCZe����!�D��	?��L��l������p�>����67V��;Z�U�c�s��P�����b����:W��fh�fu;�isk<�\���C7�\z@��U9��vE"'9�:g���ܨ:,�{+�G�iˋs"VU����h���e�ܹ�b�$�b�����W8�y�.f�M� 3�69�H���Ji��^8��B�<�!�Y�F�����	��%��%�W,S������S����Y�u���?q�t�l��x��2�W@F����4��8�PG<��Cx�Rt=rVꆞ��[ϵ�/�L�{p��ҋv��A�����6/An_U�o�[N��7m�t_�b�Q��/��I��2��O.�������7�y%���qu�U��ߌ�oF�Ys|o�6������W1W�%O��`�T���<M�I�|"oO��S��Z~��^	����剸�^�`�_�jç�(�`�v#�Q_��+��	*��떰yj�����CDe['p$#�Ё��W��#�ї�M��z�\@�9knkK�+g�9
�4Q�?p��:�u�U0G��r_�*mn;�s���ϖލe�4َ6l+��E�!O�pk}}�����      9   �  x��VMo7=[�b�)Е�_�-@z��5��`��R��rAR2�S%�0Ѓsj�@���C�����o��*9M!)��^�̒o�<�p}��ƚ�~��7��_���0�o���4��ȏ��I�U�#x�T{x;���!SY�tٷ��MG8Փkm�J���Ѕ��e֭wE#��E�E�(�OU�ԍ�螲JI�S��ȓR[�2"�
'��7~��^f���;`��e$'� �3x��w�@��጑W{~��!�r���s��"£O�Qէ�xi�j)���8����m��6/�����cU:~��n�x]ظ��h�JE�F�N�2~�,śm�k�����ŝ� ru �?���RH�OAe: 1�r>�{�ψ��(D�4�8n�����d���: ��9��WdS�RQ�e:�Nhd�s�� �,�c�*��&�0wsg�LY+�{�_�A�"sSRe�D��j\b'��LK���f�\����Zmib|��n��|�A�����}R�����>�@GDӍ�g�l�����j�qǦ̪� ��)2�#,4[���v�gy�ms���uGP�H<n�roA��d��U;����i�����ɠ`��Jcd!��f"m.�Toc�V&�|���� �0��_ �UP��/�������[�E|	:zO��v�������G�1�$F��%�w�W��rج�2��跥0A���Q�8.�(��\>��s��w�}?��v�M�G��a�?���Z��Pv���[	~Z�C��UB(�oԺ1�f5᪏��	��v%(��:�x�P���
'MO�m�Ѹu��6	M/a�6qey�xN݃H�WTl���1Nr�A��"�'��9�@�J0	\�l��U�ݤ�{�1=`H/P�CR�J�>
� �$s��L?�e_�~������D��N��X]�������^���MZ�      @   �   x�u�M
�0���)r���g��s#X���Bݸ�]	.��D�U�ԟ�W����"�./��7o<��^���j�3��p�[<
�?��$ɼ���tQx�-$��,
Sw�D�h������M��2v�Ʉ�v���^�$d�1\�@0sd�cKeg�oe'r+4d���t����%�V��z�V鼓X���9�O���      A   �   x�e�A
�0E��)r�"�����i�֍ .�^O����6�
n�oU]�a����t�p���+�������t�pB-���Bɶ�x�W\�3<�����R�L�aq?_�2�bz��gxcXK�=�p�-������G=U8"�a������4Rp:�JN�'q/S�:���;�b��~���      D   +  x�uQ[N�@�ޜb?A�����]8L�H<�J �ڟV\!�E	�\��c'�
���3�팋�t��(P�SPn��9�xּ��~2.�G�
�TZ���I�xf�l`���	�s���%�%U�V2�X��;^�X��B�x`o�:��W��%C�7����Ƿᥥ
�oj;y�_�ݱ���.��H�ɴHA���2�
Vtx|�)gr�x�)�c&�P��]+��ĳ��RŤ���!�"d,IvI� �؛���+ԇ1�_��rS'��V���=?@���*xie庒wL7����$��Յn�      ;   �  x����r�Tǯ��8ε�X���\r���S&�In��21��d(�u:\@q��ʎ��7�{$YR$��Y:g����vW'�E�Xm)�9�����P��o03��6��q��5��z@�L�(P��k}�Y �n]�F�;�܌�����>��w<�`t�����h�+�{u�'�,a����K5��ݷ���rM�mY�}������>��h�����������ޗ��a�B������KW�x�sߢ���ӈ�E.��MMhJc\Gr�Ua��.0x�s[�P�^�3���S	1��Q=�O$�X��h�a��
��~�5Cڙ��C�$Hk��_���EӿF����Ɉ������=�F�O/���ݥ�E�f�.w��8�'//����=N�	��/�����x�"�@�ʡ{�W=�I�?M�|�
��'���9E�	�)=��>�@Q���6�ٖ/ْ�ȯ��x�M��(��$vv��f�K�t�`��Ʋ�����Qͥ&o���s�BM��,��4�nV�uc�o�ʏ,J?����W&~�&%S}O�� ��$���^F�YCDپ��/�+.=x�6�׹������;ݫY�'ojx¢�(��ϰ�ɀ�{1@c�oI��N��9��/㛠�U`b��q7�m�r�g$�	�8��kq�"g���B��-s�-�_��WS������0�A�Z!5�3�o��-ω(�b����_K��W��s�/��/���id�-��,�i[�]ĥZ��\��cK8��9��V�X��~W�{*vF� d�R�[�r���0���A��>AS�`��I��m+n�l�Dzb1�5o٢�JU�z�t���Y/r&�مi^vM�y��	���4�a[X��Y��v�X'a�..nd"X�%Ǎ��B���x2n��)z���G��0r�avB	}h��v��H�z(�TTsȄ;�O�וqT��,V�W��쎣D�it�puW�WǠ�T�u\Q�q�[r��/�TH@y�p�0Qܠ�(�b��� �>/���XNn�|���N
�ka4���$��-x;��N�]z��E��S�`�X`������a�
�n9��Aҭ��$�FQ�Ga Ҁֻ����>�c:�A�����_�녿�����%�8�E���ٶ�?�Q��      F   �  x��Z[o�~�~�BO6@R˩�<��F�%�,F)P�70 ��l��6*���E�WiRI�����������%8N�,K\�Μ�w�sof{cmfm�8�����s��5w�3�p.��U�[�������8h����^���`�e�B��o|Ͽ��}��w�����7��6��g�E�~:�hL��f���>���;~����w��T��K��S��il9/�g��SwV\�]w�
�o�e�Y-�ŊSv����F�V)U�9�J��[�RyG�����-W����F��l���lT�rk�Bafyf�>1U��E��(m��m�E��;D�����_�W]��H�#�:$ϡ_��I���@��-b�>����d�w���h`VW�tW�8���Ep��=��{�p:�G����9pDt��~��y�0>x:������~9&����{@O���[b�ٿ
vYy�{�����X��0D�� �!���x18 ��'SN;6YL`b��n��BÎ9G�ǿpL�m���[��~�NNE��+�v( نI˧JIA�N<�j�4��H�v@�w��ADؼ�@q�'�bm}� ��uD�F8Z8��(9�˶ʀ%�+ڞ��WB��A��N���r+u���d��ҶP.�C��2#N�!�i�1��e�vˇ^0iyPED����+��͕H�%��^]m ��Ҁ�A�L�p�o(i�7�H��W����8	YA��x	�<��b�Qs�r3?`�s��c�H��	m�Z:a�`>H�W01��l9����j�����Z2���T��
G5��w�S���1�,c�'O��Eo���i�V$�t�d�Z)n-h1�O_r����L�T�t֋��r���P*�3����ǳ��-:xD�=`��A�=@h��}�1���UL�&c��ja�,�(x�Y~e{�!��5��Y��6���oiy�6��F���w�n#_z��z��ܭ��Gխm��VV]H�����b��OY�`�ʲ��<��M�;8j�����2tC�.��-�W �=~��KSdy�A m�r�y�x���N����{�B�����"'Ǿ��++�\�Ŗ��{�1\��9}�s�܃��x����G �C�1,�=�3�5��O���������0v�!U�d<
D���S:qg�]-�H������JG�4�0$��1`w�Tu(���1mT8$�8`M$����HX�*M
���,%m����h9Q�+�a���(~�A�NV�u�`�ش�u9&@}���sdC���p�]Ģ0.\��Hh�ˎ�3�A5�����՜ ��`�]�xU@~��	,C��T���j1��e��β�V�'��&'W�r�� ���^�;#��$+��:Cs�NMo����3�`��6�oh���!KB������C�
ew(�m�>�L�a��+B�t������Z~�qJ��������ؚB����T�#F5A�A�EhS��,��O1Mč����Z�)r(ے�'Wq/h`igH��1A٦m0)�DLԟЉB����C !*R��Q4���DH��tI�Q"��͛8�t�G�~�Ƒ33B[4%G���o��6�D�um����!�(B�v�#�j�r4�B~ƺ�幸`0wm��"
~������Zu5���С��:%�9p!g��!���_�!4�5���p����1T�f��Ѕ4��n�Ydǡ�d��3��,>(,,}�f�⃴#�LL	�1���j�u,�N��	��;��m��:�$j{F�>�6�}����H}�y�H9"�dܔ���|"3q�ꈖ�F	}^XЖI_�=��ZW�͠�l��>�� �u�UJ�r��������O��
0=te�:��ѱn36Nʪ�b��Ũ��D�8xhi8�U��q���= �Ȏ�_��<�ą5h�;P�3a��.Hm���>��̂�|���	����zh�MŚ��u���u��������S �ėd���M���JlP�Rd�(K�Ò'yE�*F'��ʌGY�KR�v6�S���H�*�_����7�����;�k�j��(�k�B�8��v~�>���|c�\-���������'�K���/ܟ׮9��ߺ�j��������[�T��6�+��ެ5��/��s�V]kl�K���fuk��Y�mѧj��%_ʗ+j�&�ר���g��V�'�\q %4�'K�'�O����u�v�_��0��8��C��P�l \{����#9���s~ ��<�&�(��.��@�0;\}�'�^0\����USY��G�i�B!��[a�̊�B?:G���j���"Q��:��8͏Kylc����x=J��0�,S�?Ǫ?/:zIp�#	ѣ�kO�f~���kkm�j��� ~@��o��dn
X/V��ʓ�Ɉ�B��J��M��ǎ�f60��n���޴*y��o��0]�>�� <-O%�O�աV�3M?[��53{}
�ʆS*��:K��H���s���\EΡzc��3�{���)pH�+�\�'FD	9�=� ����{�J��.�&'��I������؞�>͢2�}�Ѩvێ	YL�������(cz���"�O�ɻ�^i����J�*E�&K�.�&n_�u���%I�Jsl��eB�hS�m�&Ζ,��f����z�X�5��tءMN� Xa�h#������Կ!TRZ<*��&�e2�hP���.��^%���Ġ�?9�O����EN�TZ��j�Xݣ�K ?�`���`�u�����4n�����*����̟�P��/��gϞK�K�2���8�)
~�� N^�z���F�U��wN/�TV��c�u=gT�2;�� �6E�7q�iD���.5��yjvaS�D��u�xYb�:KPi�0��\v�F�J��X:����U8uf���I�w�#���4�|wWWf&��<hUI܆P/=8�X�C�/~�$~�óT*]����.!m(����3���F
׶Vrh��o�Cr2���D崸��.��3�\$!U�*l��%������*���c)�Mu#�W83x�2�?�FK2�Hf��=ȉ3D�<9����Т�` �m�c��k�\,i��2Rv]=&�\!���+iAH��簯�xP:S%$I$O˭��db������c�T�b`�H3R�ѧY�ʶ	 7}�J��n��1�K��Vv^���ǧ�(vZ�pD�Hd�mP�%�z�9;�W5�F �g�遃�������o0شnD{~�4e��ޡJ�Y5��|���a���$>o 9G�ᒍ_�L�S/=��H*ȉ@ :ޕ��@�aD�2Rנ����AyQ^����n��9]c��p���u��GʐGĖu�e���(i١�N�Q�ȥ7������Ҟ�Ȣ������T�n� h�3mD��fiW��e�-��"D�N`;v�Ҹ�Dj��{��7���B���	L�$�a�ddNϙ�\��9�M>Q��;;�K�+M,�i#OKO���d=;q�ж m�ۻ��wAo������3��r���{�x�Iavv�ߣ�c      H   �  x��S�N�P�o��a�P@����Iq 1h�8�a!��Ȥq1�Z�@/�p����"iCIth��{��s~,a��v���s׸�&j�$9x�4����8��Hyx"�����yAj�sO��.�ri�p�� H6�j�r��C��|��!�TH�8TԄu�y|���j	)� K_Iӿ� qْ�YlJ��L�҃��0����b(��(�tXMS|�0�[�ЉC F���|����1��J�E���MYc���/�A|�i4��&�A���Y�k�5��!�؋XK#�T+Е�>n��[���8�I��%=r���%bG��l���q��u�h��Zpw=Æ��v���F}�dnz�K�?�rbSz2�z�\��2�Z�$����XG$�������*A��m�u�fuOh�'@�d�1�{��(V[at^�@��M�첥+kY�a����h�Lȳ�      =     x��VKn�0]����V�q�d�,�k�`Ǳ�F��jZ�����W�(oF�%r�fS @H������Цݸ����{~�����i��?�A;{M����4��RZb��#M��Q��j�y�ц��KP�L��ozg9�=��`�5o�����w���lh��i�1?��GZ(�4s����Y!�bH��~�oez����8I�6��1�D�w�#;���!��"!���|r�q���&���J$���w��΅��L�Y��T�+��I!؎e�;��q��1��2���+���風v�0��N��~��8׉8ǘE�vkQ���h����}���	���nw����<8��ym����@
54dS2��f�T��L#`��v4/WʎU�B����*<D�#;�*T�Q&��V̛��0w��KJ�`��&v
�dFp/兀���.�>�p����)�吰J(f1A�	����^��S�)Uc�X����$c��c}-���
T��!.�� �=�.Q@h�%�)#L�&r��?�$-��oʆ;���P�@&{3S�6Qe�
��u�:W�ĕ�����-��H�������k���[��1U
�R(�$|$UY��Q�k �]Զ�W�ր^+�W�w�?��Z��z j	��o��=^u��J��Y��J�U�yΜ5w��vn:���٣�9O���4Jt�v�P��s@vF�V�e1p#G��7�WUZ[���E��Wi����x��+����f,Q>4xn�u�f�h覇�!����ٲ v �m��"��Ch>�o�RO4H�      B   �  x�uR�N�P\�~�]jbK�C�O�1���؅X6&&&�K�Z~�?r� ��(�=gΜ�3�F^��Fj<�qN�؛��I�j!�|Ɂg�[�Kb�\��@J]�W�D+��L�ɭ�we+{�����'���R�QG����k�g:� �U�!�O)	�+)0R���>2�!���K�t�j�w�=tuce�trEhѥ��aJ]a���X�a<p�n )��L��O�j�˺N�������{Hqp	]��/��K��|M����Q��t�u��v�8�+g�R��h��0"�`�:�r�@�/]�dA7儲�L��2@Z��I5�͍�����������)�u���l�>��Ǽ6E�Ѻ��ߙ�pօ�7��-ct����;0X7I #��}��<�����     