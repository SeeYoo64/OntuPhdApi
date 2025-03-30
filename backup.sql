PGDMP      2                }           ontu_phd    17.2    17.2 5    ,           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
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
    public               postgres    false    220   OM       #          0    17828    programcomponents 
   TABLE DATA           �   COPY public.programcomponents (id, programid, componenttype, componentname, componentcredits, componenthours, controlform) FROM stdin;
    public               postgres    false    223   �N       !          0    17812    programjobs 
   TABLE DATA           7   COPY public.programjobs (programid, jobid) FROM stdin;
    public               postgres    false    221   dP                 0    17797    programs 
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
       public               postgres    false    221    4731    218            '   =  x��Z[o�~�~�BO"@��b�i�����)PE�~+
�"Y	 ��ʒhP_?�//&M�.�������|3���=g))��K�^Ι�73������O?�ɞf�(K�q6���g�%y?�fY��*�fi��;y7��_K���x⊮v�_?�;�H���v��}lBwW��~�G&�?�{Q�·�!ɇ�Z�_Ћ��~tH�!�C;oX��u����9�ٗgH�lٌ�>�I+�'�X�zB+b�'t}-+����W$bF;�h��#n�$8���..#y<��z$КǢ+0Ο��/¶P�D ��M����q��F��K�&�Ƽ��<�cSMHm��
2�����O#~/?��m#.g��Q����8%���������ǴT�]A���W��B��SZ�7O�&y�^I�)=�����
X��&��GE6�0�*�l��o��+>*�r�a���yA��P����!�x����/8�ؾ�缋h5&��,�	����=�;�2�H�f,��l�Kt���fK�t���ї��6����`xw�!hO��0�=�^���o{�=��'{����]�)�����O���_}��ѧ�M\Z<���/֡��@��j�)�Hz �BL���g�Y���L����� e�H7]����z�-��	B�#�����%6b�F��	��zl��=X�����S���i��JRhGڃt吅��ҒGY��f��� �$,Iq6���3J��d/��ތ\�<S�S��:��ExJ!IV���;r.�I����b��!����q����e�ކn��bPq�T�����g��3{�4 x���e�L%!���w�yAV���w�D?εCF5E%=;s���
yy� �1�mjmT��.�,ik����NlxM�����s�7[���#����H�ͱ���"�QD"Ni�d�fGM444���m=���
�i�"�Jb0n%fݺl�[x���jP!��L�)����HnW��kaV� �o� �	0�����gA�y���DQ��,��J6$r����em��ؘ���͕���&X�M�@��Xȝ�5И+�P�ߎ�0vjd����Mn��hU=C50���BF�h��1'*�s�Uĺ9{	F7�G�F��?( ��	ܫ�w��^����O�~��@���l�,���zA�r�S:3��c$��drB�7��`")>�3DT,rqx��������qt(�>A��i�
�,�������2vcAX�פŹ�f���X}܅#h\|UdUC��>��Y��;IT�{|쁞/��PCէY�T�K_�Xbzj�&^�8$
�YM�|�bW�+�"� �ptt�BqE������a37]D��>l�?:n�;lݿ�9���TP�H�L���L���$���'J+�������ᇿn�X*{��
�Z�P{���!�X'AI7��c�r��ԉVc�Xw��.p��W�(-gq�;�0��ڨ%d�o++DKgS��pWjz꛶c-�񵩲ˉ�i���(}��L�ꃂ����Sa��׊����9�9aUO����#�_ySHϦ��*�F.6��F�iԸ�	�Nإ�����\���NI��
G�	ޤ�ض���{:R����5���k��gP+�\C� -P�!ua�I���Ȅ���>6�:n��v��	����\)��l��Le|!�Ep����᷂ca�����?���3r
M��K���-�.�u�����'d��X�p ����ȷ�c%\+�V��עtfGx��܈v��S��B�>���f�%�(��/��*�y�FZ���<��PEfAN�1Ro�@d7��!;�N��$&c%M��1�9g)D'`m��RT���ig;�M�ar6�0��-�S���e�>�[D�Y��m����(�ae�z �Ep7���sH�}m?ꍢ����o�*&R�Ne��ͫ�oR���b5?^b�F<�f�$͝d��MD��<�J�<E�還Rp�M��O�cZ�[�^��T�s.'"�������U6h��R����k�J��?a_Y��o�Y����҃��Z�VU0�Ȳ�M���g� E;�yy	\E4AgRId���ٶ���Fޗ���a����!h}�B��LLO\,m{d�k�;E]/�@�������ft�1�~p���dv�v>����O�l�����cB�Q���Nc�h�$Wg�A��Q5#���i\[D;2�K��Q��zQ�>��D�=������h���rӨ�������s �W��ٳ�y�a�<�'�����o�H�j (���>@�z~���ă���d�j���84c��Pw��w����fęA��|�;����)�!�����N*�@�����W��w�[>l�^��C�����Q�{c��/� �kg�'��`�mM.�OKC���Nf�C�;�Pn%��q������SOoN�#P�Jхٛf�E�Z'���f�Cݠ�����p�.��Y�9���[�m'g�3�Q�jf,�3��*�Ϳ���i��t]�5�c�6�anF���W^�霶(\$ɱ�:�L�� �mi)e$'m0�!��f�����x<o\D[l+�;gr�����0`�{V�h����Ep�xQL>�˄�����"��J+_N��ǅ]7�ʝ1��-b��$��rT���x�e0����*����� b��֔����t��s�&�Jb�
�Wʌ��Liq�<G<%��zl)�6szuC�SO4�)����t��x��0�:�g�ۼ`r��?y�nR۴ɗ�Q|�K����_��ϳ�f ԟj����3p*�x�u���֗�ԡqw�<w�om��ޞ<��������Z
j�      %   �  x�Ֆ�n�6���Sh9*���wW`��&�v7���h��,
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
��=^_{"�.E�V�vUX�� �WE8��l������g�͍�߮�7��67�͵����V=v�w����.�^^o<�����ږ��G��0*maV�n-z�ն(��yO�"w\������L��ؙ��koEҖ��cݦ__��n<o<{�4�&�Z�s}�U�۝Ɠ����Z�Uc�i}���ئ�dm�I��F�	__�o7w֚��[\�u��� g0      )     x��VKnA\�O�K�0�L��� ;�`G���	R�H�.`Of���+t�(U�{�3���R$O��իW�;a�q��}#
�v3�6��q�����33�'fc�l����+;�f��+�h,O�I����dfmOll�+��f�+�'f��16�q;~��ߩ���g4�	�����,�js��m�F�)A�in�W���Taw�G����$��sv"	�{����l1�K^�.XC>	G�C&)�q5��V9��y���Ĳ�N9� �9ƹ��479��a;�]op~���R�L+�88���
�>���-?��W-�_$%�:�1sL)��X�re���ڌUT�Q��3�ؙ�����9�a�m�a����.u���Ҕ$x�锚��4&ݴ1�j��H�hF�&��u��D���t�Ϙ�v�cE���5 }_�����U'�)[Ov&P"-;�d���E+i6�8=�t�7x9�E����uF���%�1�� eå�pΤ߆t�eb��YS {�u�Ɖ�1��U��n�l^�ӾީF�Ru�	�e��~hG�\kW�( �b`:��ve��������1�s]����V��E�;err��`R��Sh�����	�V�x��>c(��L{�T�y�/�+�1����UX�J������=��X�?�q6P�����%�oE�jWU�]$`ҩN����0��?ڽ7�IY�K�[�G��1�M�_��m��ZWaX��z.�j$�#��z��k ���}y^��;�R�Q�~����E��S��*:��ݿ.O���k��=<EH�     