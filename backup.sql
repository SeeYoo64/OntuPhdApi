PGDMP      :                }           ontu_phd    17.2    17.2 5    ,           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            -           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            .           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            /           1262    17788    ontu_phd    DATABASE     |   CREATE DATABASE ontu_phd WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
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
       public               postgres    false    227            0           0    0    applydocuments_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.applydocuments_id_seq OWNED BY public.applydocuments.id;
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
       public               postgres    false    225            1           0    0    documents_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.documents_id_seq OWNED BY public.documents.id;
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
       public               postgres    false    220            2           0    0    jobs_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.jobs_id_seq OWNED BY public.jobs.id;
          public               postgres    false    219            �            1259    17828    programcomponents    TABLE     i  CREATE TABLE public.programcomponents (
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
       public               postgres    false    223            3           0    0    programcomponents_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.programcomponents_id_seq OWNED BY public.programcomponents.id;
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
       public               postgres    false    218            4           0    0    programs_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.programs_id_seq OWNED BY public.programs.id;
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
       public               postgres    false    229            5           0    0    roadmaps_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.roadmaps_id_seq OWNED BY public.roadmaps.id;
          public               postgres    false    228            x           2604    17881    applydocuments id    DEFAULT     v   ALTER TABLE ONLY public.applydocuments ALTER COLUMN id SET DEFAULT nextval('public.applydocuments_id_seq'::regclass);
 @   ALTER TABLE public.applydocuments ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    226    227    227            w           2604    17883    documents id    DEFAULT     l   ALTER TABLE ONLY public.documents ALTER COLUMN id SET DEFAULT nextval('public.documents_id_seq'::regclass);
 ;   ALTER TABLE public.documents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    225    224    225            u           2604    17884    jobs id    DEFAULT     b   ALTER TABLE ONLY public.jobs ALTER COLUMN id SET DEFAULT nextval('public.jobs_id_seq'::regclass);
 6   ALTER TABLE public.jobs ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219    220            v           2604    17885    programcomponents id    DEFAULT     |   ALTER TABLE ONLY public.programcomponents ALTER COLUMN id SET DEFAULT nextval('public.programcomponents_id_seq'::regclass);
 C   ALTER TABLE public.programcomponents ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    222    223    223            t           2604    17886    programs id    DEFAULT     j   ALTER TABLE ONLY public.programs ALTER COLUMN id SET DEFAULT nextval('public.programs_id_seq'::regclass);
 :   ALTER TABLE public.programs ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    217    218    218            y           2604    17977    roadmaps id    DEFAULT     j   ALTER TABLE ONLY public.roadmaps ALTER COLUMN id SET DEFAULT nextval('public.roadmaps_id_seq'::regclass);
 :   ALTER TABLE public.roadmaps ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    228    229    229            '          0    17871    applydocuments 
   TABLE DATA           `   COPY public.applydocuments (id, name, description, requirements, originalsrequired) FROM stdin;
    public               postgres    false    227   B>       %          0    17857 	   documents 
   TABLE DATA           D   COPY public.documents (id, programid, name, type, link) FROM stdin;
    public               postgres    false    225   �I                  0    17806    jobs 
   TABLE DATA           /   COPY public.jobs (id, code, title) FROM stdin;
    public               postgres    false    220   hM       #          0    17828    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, programid, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    223   �N       !          0    17812    programjobs 
   TABLE DATA           7   COPY public.programjobs (programid, jobid) FROM stdin;
    public               postgres    false    221   }P                 0    17797    programs 
   TABLE DATA           �   COPY public.programs (id, name, form, years, credits, sum, costs, programcharacteristics, programcompetence, programresults, linkfaculty, linkfile, fieldofstudy, speciality, name_eng) FROM stdin;
    public               postgres    false    218   �P       )          0    17974    roadmaps 
   TABLE DATA           ]   COPY public.roadmaps (id, type, datastart, dataend, additionaltime, description) FROM stdin;
    public               postgres    false    229   �_       6           0    0    applydocuments_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.applydocuments_id_seq', 1, true);
          public               postgres    false    226            7           0    0    documents_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.documents_id_seq', 11, true);
          public               postgres    false    224            8           0    0    jobs_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.jobs_id_seq', 10, true);
          public               postgres    false    219            9           0    0    programcomponents_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.programcomponents_id_seq', 22, true);
          public               postgres    false    222            :           0    0    programs_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.programs_id_seq', 2, true);
          public               postgres    false    217            ;           0    0    roadmaps_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.roadmaps_id_seq', 13, true);
          public               postgres    false    228            �           2606    17878 "   applydocuments applydocuments_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.applydocuments
    ADD CONSTRAINT applydocuments_pkey PRIMARY KEY (id);
 L   ALTER TABLE ONLY public.applydocuments DROP CONSTRAINT applydocuments_pkey;
       public                 postgres    false    227            �           2606    17864    documents documents_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.documents
    ADD CONSTRAINT documents_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.documents DROP CONSTRAINT documents_pkey;
       public                 postgres    false    225            }           2606    17811    jobs jobs_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.jobs
    ADD CONSTRAINT jobs_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.jobs DROP CONSTRAINT jobs_pkey;
       public                 postgres    false    220            �           2606    17835 (   programcomponents programcomponents_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_pkey PRIMARY KEY (id);
 R   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_pkey;
       public                 postgres    false    223                       2606    17816    programjobs programjobs_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_pkey PRIMARY KEY (programid, jobid);
 F   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_pkey;
       public                 postgres    false    221    221            {           2606    17804    programs programs_pkey 
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
       public               postgres    false    218    225    4731            �           2606    17836 2   programcomponents programcomponents_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.programcomponents
    ADD CONSTRAINT programcomponents_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id);
 \   ALTER TABLE ONLY public.programcomponents DROP CONSTRAINT programcomponents_programid_fkey;
       public               postgres    false    4731    223    218            �           2606    17822 "   programjobs programjobs_jobid_fkey    FK CONSTRAINT     ~   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_jobid_fkey FOREIGN KEY (jobid) REFERENCES public.jobs(id);
 L   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_jobid_fkey;
       public               postgres    false    220    221    4733            �           2606    17817 &   programjobs programjobs_programid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.programjobs
    ADD CONSTRAINT programjobs_programid_fkey FOREIGN KEY (programid) REFERENCES public.programs(id);
 P   ALTER TABLE ONLY public.programjobs DROP CONSTRAINT programjobs_programid_fkey;
       public               postgres    false    221    4731    218            '   V  x��Z�n����b�+X)�b�is��Hs��"	Z�DR?6 ��ʒhP�Jsы�P)��r	�/��
y���3��;CJr��E� �D�Μ��|���m|���y7�L>(����� _��M�MM��&���2��hբ��M���Ű�z�V���9��O�fё5Y~�OSthZ�V�|�O��E[��6|*�W�G��$ã�k:�4϶I�!-��G�$ˌv<f�H����=Z0�Ն����^��m�A��$Aq�l�%�:�U����X�1+P��#��Џ9�6��mҼ͟el��Om�V���'&˒ʴ��NK�ey����e�"�g�����H���]����C̯;��y�铧�r�7f+��W6�Vb���/��䛧O��
_��H��ǃ���tޘ䤍`Yx!��_���>�'������H����iݠ�� Lыy��n#~��e_t��4b��!�I5��'r�zi�_��$�����h�vڥ[I�o��yLN�J^��Ɇ1C���|���Π��GI�>ٲ��E�83����-�&>H!V���S���Dl:D��5Z�OZ4��	�����Rt)NXR��֖P����lSS^�<=>��T�c,��Ί��d�iM81BL83���vT:r�Ǯ��?a?-4]D4�^�N7�)É>����'��r�2a�44~��T�x�jM��%Y�%&�Ð��i
L���g@L� ��!����}�$×��@��A�44�0�Mݐ]�]�~ܞ�����a8�+��H�2eסwюYl�)9����'"i�@<�@gr���x�Ϳ��쐞��,���~D��)w��O��R!F����Z�r%��֟+`Q�t� PxY�����A$ي>>��S9v&��y�}#�L��cd�Ɩ �QHT���=Ϗ�t|L���ϖ1Nč7Ǖ��>�L���+�<�+[r��ư6K�3���J��e��l�H-H�	�ZW�k{�VZ-P)ao�8�,�!�S�Fy��#N�$���M�`U�+O�=uLi=�9�x=�A} �Q9q���_�[S!s�9�h�GT��yn��M�J!3'�+|���k B,1��?��Z��E<�,(-�#dT�v?��@Ytj�p����YSD�L�ʋ1��]	*��
������H��s(��)�68��$:3p��;�t����D��|r��4�`��r�n��it�
���c�J-`C��@,Y� ��w��1��3�y��x��!G.��TslX�VVVrL?9I]���K{ۂH�͕�O٫P��%�~*4*�jflDv�B<Z���R�
�]͞����f�8�ƨ0m�hb�ӊ:�#"ҋ����$��-mQ�zy�c/�8�F��P��#Ž�5-�D��15Q&-!�^�3���w����%������������V�氘m/$��r����	}��r�p����).7��p�$v�?.��(�2��;�j۵����9�J|K��z/�
�x핖�p��}������&���v�T��O��J���Jb�KRZAF��H����:G� �r�R!`SH1p�����ۣ���L����'h��b��-�Kj�.�uB��V��u��r��֫AĽ�-�J	���0�eH-K����m	��RZ�iR�7N�Z���h}�U΅G�Bi�ߣ ����(?�:��Ajg5�8�L�yuӽ�vf2my��l�S+Vt�[����n/�}Z�4��罹J#ί���e�rzs���_l0y���^Bg�>�qx�E�u���Ԟw �
V���-��GQ����n�݃�jb�Ks�y��H� ������|�13׷D�-{�����C� mS ����L�8�`��J�w�
a0w }�E��C_���@�$����M��T�������ݝ��œH��{���������UU�a䦦�D�.�D�R�����Ќ<+�����s��2�e���L��-Z�T;U,�R_����F�?H%�i�o���Ɂ�$���]K|��Vw-��˖j�!񦀛����d��?�
���x@!g�I��L`��wmS�2�t|4.�<���l�mW����5���n�
�������&x�66c\�~����Gnvg��n���� J]�0�+bI;�\Wٓr�6�$<1�1���(�XŰw��P6�&�}]f`8 Z�H�V�c]X���@�B�EM̀�
ւ��gA���m8�vW��9�d2�R֍�A4�bc.�r6WƲ��g`��Ი;eBs)�[�_C���l�j�{�un|��t�����.�u5����ɠ� �������$������nb�M�;f��'ۏ����n?z8僽���!d��P��Ǆj5�DO��N��"�Y��7;�r-*  Vm���q����Bd���۶c.��gW�.����6J���c�CK���\�xE8�Ja��(>�/N�CƟ#��`�HK�gʿkM��Ư��X�ra�s]�5>� ������^�c͝�L� +#� �<�봝�����V��^��lw�ji�R_�c�B?��d��,}��
ׁ�)��k�ب��ӤC����ʥ��ܗi����Bʋđ"�{~Q N�wQc��=x?Wu|����A���,�ti�"&/f�e&T�Y�Wg�����"��H!��N�����G_�u� ��"ߦ��%^H����;jsU�8��GHz���!�v鸯w{�bB�H�f��M_�2�z�j��T�`�{��<`�XI�;AH���D}Wb *U�y��J!o��c���|L�r�-k{�X:(�0"o&t䥏�R
�w��T��nA���3���נ�נ�נ���o�Q���ю�P#�7��5&�|�w;���?nRi�      %   �  x�Ֆ�n�6���Sh9*���wW`��&�v7���h��,
$� Y�㦃t�Yu
�/�8N�ĉ�
��\2vmO��1S�]D�/��s?]r��t�f�3;������[w�;���";r�vlǸN#;t}�O12Dҡ�n��)3{��lu��-U��㵎1���ٔ��<�kfr��%˚e�L-�,��	-d�d25R�<)�6mŲ����w�2k�׶�j�ܝ���b�z� tؑ��6{�G�� ��{������^F���F<�B����B�RT�~ ����i����o�I�#7��+������)�2��R�)<&��#���@�,s����8��ˎȥ�eg������*�X+���Fat�˙�:��>��<y�mH}��U���k�6��`�R#R?	pvV�|� �5g왝J�lJ� �.p?�z���7qGQ�Bh�}�a 8�h�^�%�/�F�!$m̔�ߐ&ӥP��*N%S$*6�F~Լ0�:�-��r�К+��?�|�܌\�/+t�)������-�Is�v_�v�y>���%�K09"gQ��O���Y>"L�~
�2�V�M�󧛱�{U掂�B��>��a��$���G�F�m.^�w�(������<Z2�-E��ݎ.xou�����!�l�4�|m� S�0�M�qڌLG��/�gaQ�~�z�ldb���&8t����
�����Ʋ��'r��n^����3�t���f�{S@߀�	��i��"_� Z{�?#��0\�߃�CI7iI�P[d*�P����>X�O,Q��E�'(>�E�^$�R"o�s/�
�䛰�l$��%B��&�<H��ǂ���&�X;��|�����q#	���?5�a�A�c/�%�<�of�Y���d�8 �i�q�yh�z{�D��;��M;\iY����{;?m���?��^          1  x�uQ[N�0��O��F��N��.�M$����B0jH�
�7b�1A�gwfvv�D&1���hC�Z��JM=��ĳ�-u��J&�f��r�����
�J-G��aVϠZ�-�-?�]Sk��'b�;�ox�����b�u\�]P��X�c�w_lY�oǙ��=��:�l�;ќO���f|/��P�;=����^�Psƹu1&S��l���2J��	ʵ�}��1�,D,F�9H xʋEh�	aL���2f%ij��װd�w��>�Q��˒�>0�,X��d�{ *cu=WJ� 0�z�      #   �  x��S�N�P�o��鬆"<�.N�@�A�Ĺ-��D&��q�@����=o�wn�����д����������]�}[���j�$�xz4����8��H�x"����ą�P�j�YU#��#Z" @ rM�����<����_�e�� L*$��*J�.3bB���k	)�����f/�ٟU �d�v�4�!�E&+	���{Xeؙ�d(�����WL3��,rS?ȉ? F���|������0��J�EP�Τ�;�V�c��k�N��į�3��4�5�q�wԐe�E,�3��Bq7-�,Y�H�l�H��%=p��q��aC����k��P9M���f�Zo�|��!�m�lx%�����t7)�D��=鹘�!�t����s����s�����F�8;u�7��u��=���p���@hZ�p�Xl��y#=wu��X�����ryd�7����      !   *   x�3�4�2�4bc 6bS 6bs � bK 64������ ���         6  x��[�n�]�_��Jhَ=��x��m�dL�F�-%�hHt �@e�)��&��|@�����|Q�N�}t��e�,��E��>�y������?y�&��Y>��Y{v�Ѕ񬝟Ӆ�����t�7۟ӧ��M�_���Vo��G�z�SK�VF�����x�����hB�S���|KW;�ޟ�tSs�nc䥯�󌞛�K+4<��Ϸ�k�r>�Sss���å��-���/��[�wo����S-y����_觇�?=^z���S_[�2��[�w�6ئ��4�VH�m����uhn��ᡔ��[������gG"��)	��#��{��	�ѧY���	�qB��9{�B�N�_&4֔�J�!�ODP�"=�O�V:tC�D�dѰ���i`���� �4퀾���h^,�}��oÄ7�+�e��z:|��{4��1���V������ [�4��{v ���r�?YȐ���?]e#h|���z�����B�%�d��Hx��ߗe��z䵼g��8����̐��"kLi0�cq�0T�?l��s�}�71�Κ���+g�*��M Z�%�(���U5�оNo)���>6K��Q�m�7{GoG5JB�
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
��=^_{"�.E�V�vUX�� �WE8��l������g�͍�߮�7��67�͵����V=v�w����.�^^o<�����ږ��G��0*maV�n-z�ն(��yO�"w\������L��ؙ��koEҖ��cݦ__��n<o<{�4�&�Z�s}�U�۝Ɠ����Z�Uc�i}���ئ�dm�I��F�	__�o7w֚��[\�u��� g0      )   �  x��UKn�@]kN1��I�=DO�e]���)`$@PȻ��	dEndْ�@ި�ɕ�)�3����{�$�>}���~���C���ػz��w��W�r�Jx�1�Y�h��RA;J�M��R�\�=-L����C77��Bb��6��}��#�-U��<�=mp/���rD�E\�w�)�<����%6�R]���9�q ��+Z�8m���\th�Z�)zC���<q��B���y�0��hM��n_F���Kh ��Ӄ�L�D&jhn�˔�p��ʡ�Ū��'\��Jx�5:o�á��X�Xp�u���%ߊ�X�$���`���Zn՘J���хX]aJ�'�(U&5Gx�%Ǧ�-��q/j���U�g�4�s0~_a�7���\(�c�"=j9"C��&ۨ�����_���'����R8[��꟫l[e�:r?J���×�#��L�P�J��l_�՞�df睂�=���u�`��^t��X���OO�9G�5��y�T���,�H�~ n"̩��U�F�U"�ҩN��������uz��z9`oշ���[I���	��օ�cܙ`h]
��J��I�h��=����@��𹳯�\n��?q��i�����m��K�����Ct�]���!���`U�MB!k�7�
j=�/�w��su2�IO��c~�*t     