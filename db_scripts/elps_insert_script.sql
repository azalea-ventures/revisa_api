USE revisa_db;
GO

BEGIN TRANSACTION
-- Insert data into Learning Strategies table
INSERT INTO elps.learning_strategies
    (label, strategy)
VALUES
    ('A', 'use prior knowledge and experiences to understand meanings in English'),
    ('B', 'monitor oral and written language production and employ self-corrective techniques or other resources'),
    ('C', 'use strategic learning techniques such as concept mapping, drawing, memorizing, comparing, contrasting, and reviewing to acquire basic and grade-level vocabulary'),
    ('D', 'speak using learning strategies such as requesting assistance, employing non-verbal cues, and using synonyms and circumlocution (conveying ideas by defining or describing when exact English words are not known)'),
    ('E', 'internalize new basic and academic language by using and reusing it in meaningful ways in speaking and writing activities that build concept and language attainment'),
    ('F', 'use accessible language and learn new and essential language in the process'),
    ('G', 'demonstrate an increasing ability to distinguish between formal and informal English and an increasing knowledge of when to use each one commensurate with grade-level learning expectations'),
    ('H', 'develop and expand repertoire of learning strategies such as reasoning inductively or deductively, looking for patterns in language, and analyzing sayings and expressions commensurate with grade-level learning expectations');

INSERT INTO elps.learning_strategies_mods
(learning_strategy_id, strategy, image_url)
VALUES 
(1, 'use prior knowledge and experiences to understand meanings in English', '1pDqnoeFh2SKxYr5SjWq-A0Lm1X_-84He'),
(2, 'monitor oral language production and employ self-corrective techniques or other resources', '1KYclczZP-Vwrz5aSAEP8fu_3kNpqojZ7'),
(2, 'monitor written language production and employ self-corrective techniques or other resources', '1Vo35Lwqm3eCQTl2Au0P4XNMGZBgsQZ4_'),
(3, 'use strategic learning techniques such as concept mapping to acquire basic and grade-level vocabulary', '1lnhU4MPeYXsUpHF8Gcq8e5Iukz5E-pTv'),
(3, 'use strategic learning techniques such as drawing to acquire basic and grade-level vocabulary', '1KwkPVKPSxazeA5lE6QxwqZUw5rOnHHI9'),
(3, 'use strategic learning techniques such as comparing and contrasting to acquire basic and grade-level vocabulary', '1iKxhbs0zsheuX7tbDb3DOulHZHBLorC2'),
(3, 'use strategic learning techniques such as reviewing to acquire basic and grade-level vocabulary', '1Cwb28HRDI7L2MhnYC-T4tb85n9YFbLkr'),
(4, 'speak using learning strategies such as requesting assistance when exact English words are not known', '1j4b3BLw2oXkA7ng_EImnVSr1gfacBbvV'),
(4, 'speak using learning strategies such as employing non-verbal cues when exact English words are not known', '12wBbnq9ye7GrECtcjD9EYEtSl36Gedym'),
(4, 'speak using learning strategies such as using synonyms when exact English words are not known', '1I1Wh6qULymtGWq33AaBET0754YoYHyaM'),
(4, 'speak using learning strategies such as circumlocution (conveying ideas by defining or describing) when exact English words are not known', '1h_3PBysne2n0VCSYozEMAlO-BJ8qSuwa'),
(5, 'internalize new basic and academic language by using and reusing it in meaningful ways in speaking activities that build concept and language attainment', '1GpbISNZaLGKh64qaFPBjGFMeiLJQtWZb'),
(5, 'internalize new basic and academic language by using and reusing it in meaningful ways in writing activities that build concept and language attainment', '1wRf6q6UQGMMII-thAE3j3vC3lVMxivnG'),
(6, 'use accessible language and learn new and essential language in the process', '1czr0oc8DIHMrQDKpDtC99wGVWlHBWs9J'),
(7, 'demonstrate an increasing ability to distinguish between formal and informal English and an increasing knowledge of when to use each one commensurate with grade-level learning expectations', '1igtMz5nvo-3WDk4Dw6ACipoaPmTmI6tx'),
(8, 'develop and expand repertoire of learning strategies such as reasoning inductively commensurate with grade-level learning expectations', '1gSRga51RHPcauZBc6BVq3Xn754hd5eqo'),
(8, 'develop and expand repertoire of learning strategies such as reasoning deductively commensurate with grade-level learning expectations', '155EOwTXGes9yyvwH7dWU2kQr2pJ7NpWx'),
(8, 'develop and expand repertoire of learning strategies such as looking for patterns in language commensurate with grade-level learning expectations', '1bFvrwb-Sv_Td2kwXNwZt85LF9PI_ccDo'),
(8, 'develop and expand repertoire of learning strategies such as analyzing sayings and expressions commensurate with grade-level learning expectations', '15UN-zIukgD1Uozny55uTE7IPjmO3uQrv');

DELETE FROM elps.learning_strategies_mods

COMMIT;

BEGIN TRANSACTION
-- Insert data into Domains table
INSERT INTO elps.domains
    (domain, label)
VALUES
    ('listening', '2'),
    ('speaking', '3'),
    ('reading', '4'),
    ('writing', '5');
COMMIT

BEGIN TRANSACTION
-- Insert data into Domain Objectives table
INSERT INTO elps.domain_objectives
    (domain_id, label, objective)
VALUES
    --listening
    (1, 'A', 'distinguish sounds and intonation patterns of English with increasing ease'),
    (1, 'B', 'recognize elements of the English sound system in newly acquired vocabulary such as long and short vowels, silent letters, and consonant clusters'),
    (1, 'C', 'learn new language structures, expressions, and basic and academic vocabulary heard during classroom instruction and interactions'),
    (1, 'D', 'monitor understanding of spoken language during classroom instruction and interactions and seek clarification as needed'),
    (1, 'E', 'use visual, contextual, and linguistic support to enhance and confirm understanding of increasingly complex and elaborated spoken language'),
    (1, 'F', 'listen to and derive meaning from a variety of media such as audio tape, video, DVD, and CD ROM to build and reinforce concept and language attainment'),
    (1, 'G', 'understand the general meaning, main points, and important details of spoken language ranging from situations in which topics, language, and contexts are familiar to unfamiliar'),
    (1, 'H', 'understand implicit ideas and information in increasingly complex spoken language commensurate with grade-level learning expectations'),
    (1, 'I', 'demonstrate listening comprehension of increasingly complex spoken English by following directions, retelling or summarizing spoken messages, responding to questions and requests, collaborating with peers, and taking notes commensurate with content and grade-level needs'),
    --speaking
    (2, 'A', 'practice producing sounds of newly acquired vocabulary such as long and short vowels, silent letters, and consonant clusters to pronounce English words in a manner that is increasingly comprehensible'),
    (2, 'B', 'expand and internalize initial English vocabulary by learning and using high-frequency English words necessary for identifying and describing people, places, and objects, by retelling simple stories and basic information represented or supported by pictures, and by learning and using routine language needed for classroom communication'),
    (2, 'C', 'speak using a variety of grammatical structures, sentence lengths, sentence types, and connecting words with increasing accuracy and ease as more English is acquired'),
    (2, 'D', 'speak using grade-level content area vocabulary in context to internalize new English words and build academic language proficiency'),
    (2, 'E', 'share information in cooperative learning interactions'),
    (2, 'F', 'ask and give information ranging from using a very limited bank of high-frequency, high-need, concrete vocabulary, including key words and expressions needed for basic communication in academic and social contexts, to using abstract and content-based vocabulary during extended speaking assignments'),
    (2, 'G', 'express opinions, ideas, and feelings ranging from communicating single words and short phrases to participating in extended discussions on a variety of social and grade-appropriate academic topics'),
    (2, 'H', 'narrate, describe, and explain with increasing specificity and detail as more English is acquired'),
    (2, 'I', 'adapt spoken language appropriately for formal and informal purposes'),
    (2, 'J', 'respond orally to information presented in a wide variety of print, electronic, audio, and visual media to build and reinforce concept and language attainment'),
    --reading
    (3, 'A', 'learn relationships between sounds and letters of the English language and decode (sound out) words using a combination of skills such as recognizing sound-letter relationships and identifying cognates, affixes, roots, and base words'),
    (3, 'B', 'recognize directionality of English reading such as left to right and top to bottom'),
    (3, 'C', 'develop basic sight vocabulary, derive meaning of environmental print, and comprehend English vocabulary and language structures used routinely in written classroom materials'),
    (3, 'D', 'use prereading supports such as graphic organizers, illustrations, and pretaught topic-related vocabulary and other prereading activities to enhance comprehension of written text'),
    (3, 'E', 'read linguistically accommodated content area material with a decreasing need for linguistic accommodations as more English is learned'),
    (3, 'F', 'use visual and contextual support and support from peers and teachers to read grade-appropriate content area text, enhance and confirm understanding, and develop vocabulary, grasp of language structures, and background knowledge needed to comprehend increasingly challenging language'),
    (3, 'G', 'demonstrate comprehension of increasingly complex English by participating in shared reading, retelling or summarizing material, responding to questions, and taking notes commensurate with content area and grade level needs'),
    (3, 'H', 'read silently with increasing ease and comprehension for longer periods'),
    (3, 'I', 'demonstrate English comprehension and expand reading skills by employing basic reading skills such as demonstrating understanding of supporting ideas and details in text and graphic sources, summarizing text, and distinguishing main ideas from details commensurate with content area needs'),
    (3, 'J', 'demonstrate English comprehension and expand reading skills by employing inferential skills such as predicting, making connections between ideas, drawing inferences and conclusions from text and graphic sources, and finding supporting text evidence commensurate with content area needs'),
    (3, 'K', 'demonstrate English comprehension and expand reading skills by employing analytical skills such as evaluating written information and performing critical analyses commensurate with content area and grade-level needs'),
    --writing   
    (4, 'A', 'learn relationships between sounds and letters of the English language to represent sounds when writing in English'),
    (4, 'B', 'write using newly acquired basic vocabulary and content-based grade-level vocabulary'),
    (4, 'C', 'spell familiar English words with increasing accuracy, and employ English spelling patterns and rules with increasing accuracy as more English is acquired'),
    (4, 'D', 'edit writing for standard grammar and usage, including subject-verb agreement, pronoun agreement, and appropriate verb tenses commensurate with grade-level expectations as more English is acquired'),
    (4, 'E', 'employ increasingly complex grammatical structures in content area writing commensurate with grade-level expectations'),
    (4, 'F', 'write using a variety of grade-appropriate sentence lengths, patterns, and connecting words to combine phrases, clauses, and sentences in increasingly accurate ways as more English is acquired'),
    (4, 'G', 'narrate, describe, and explain with increasing specificity and detail to fulfill content area writing needs as more English is acquired')

COMMIT;

BEGIN TRANSACTION
-- Insert data into strategies_objectives table
INSERT INTO elps.strategies_objectives
(strategy_mod_id, domain_objective_id)
VALUES
(1,1),
(2,10),
(3,20),
(4,31),
(5,2),
(6,11),
(7,32),
(8,3),
(9,12),
(10,22),
(11,33),
(12,4),
(13,13),
(14,23),
(15,34),
(16,5),
(17,14),
(18,24),
(19,35),
(1,6),
(2,15),
(3,25),
(4,36),
(5,7),
(6,16),
(7,26),
(8,37),
(9,8),
(10,17),
(11,27),
(12,9),
(13,18),
(14,28),
(15,10),
(16,19),
(17,30),
(18,21),
(19,1),
(1,10),
(2,20),
(3,31),
(4,2),
(5,11),
(6,32),
(7,3),
(8,12),
(9,22),
(10,33),
(11,4),
(12,13),
(13,34),
(14,5),
(15,14),
(16,24),
(17,35),
(18,6),
(19,15),
(1,25),
(2,36),
(3,7),
(4,16),
(5,26),
(6,37),
(7,8),
(8,17),
(9,27),
(10,9),
(11,18),
(12,28),
(13,10),
(14,19),
(15,30),
(16,21),
(17,1),
(18,10),
(19,20),
(1,31),
(2,2),
(3,11),
(4,32),
(5,3),
(6,12),
(7,22),
(8,33),
(9,4),
(10,13),
(11,23),
(12,34),
(13,5),
(14,14),
(15,24),
(16,35),
(17,6),
(18,15),
(19,25),
(1,36),
(2,7),
(3,16),
(4,26),
(5,37),
(6,8),
(7,17),
(8,27),
(9,9),
(10,18),
(11,28),
(12,10),
(13,19),
(14,30),
(15,21),
(16,1),
(17,10),
(18,20),
(19,31),
(1,2),
(2,11),
(3,32),
(4,3),
(5,12),
(6,12),
(7,22),
(8,33),
(9,4),
(10,13),
(11,34),
(12,5),
(13,14),
(14,24),
(15,35),
(16,6),
(17,15),
(18,25),
(19,36),
(1,7),
(2,16),
(3,26),
(4,37),
(5,8),
(6,17),
(7,27),
(8,9),
(9,18),
(10,28),
(11,10),
(12,19),
(13,30),
(14,21),
(15,1),
(16,10),
(17,20),
(18,31),
(19,32),
(1,3),
(2,12),
(3,22),
(4,33),
(5,4),
(6,13),
(7,23),
(8,34)
COMMIT;

BEGIN TRANSACTION
-- Insert data into Levels table
INSERT INTO elps.levels
    (lvl)
VALUES
    ('beginning'),
    ('intermediate'),
    ('advanced'),
    ('advanced_high');

-- Insert data into Grades table
INSERT INTO elps.grades
    (grade)
VALUES
    ('K'),
    ('1'),
    ('2'),
    ('3'),
    ('4'),
    ('5'),
    ('6'),
    ('7'),
    ('8'),
    ('9'),
    ('10'),
    ('11'),
    ('12'),
    ('primary'),
    ('secondary'),
    ('all');

INSERT INTO elps.attr_type
    (typ)
VALUES
    ('sentence'),
    ('list');

COMMIT;


BEGIN TRANSACTION
-- Insert data into Domain Levels table
INSERT INTO elps.domain_levels
    (domain_id, level_id, details)
VALUES
    --listening
    (1, 1, 'Beginning ELLs have little or no ability to understand English spoken in academic and social settings'),
    (1, 2, 'Intermediate ELLs have the ability to understand simple, high-frequency spoken English used in routine academic and social contexts'),
    (1, 3, 'Advanced ELLs have the ability to understand, with second language acquisition support, grade-appropriate English spoken in academic and social contexts'),
    (1, 4, 'Advanced high ELLs have the ability to understand, with minimal second language acquisition support, grade-appropriate English spoken in academic and social contexts'),
    --speaking
    (2, 1, 'Beginning ELLs have little or no ability to speak English in academic and social settings'),
    (2, 2, 'Intermediate ELLs have the ability to speak in a simple manner using English commonly heard in routine academic and social settings'),
    (2, 3, 'Advanced ELLs have the ability to speak using grade-appropriate English, with second language acquisition support, in academic and social settings'),
    (2, 4, 'Advanced high ELLs have the ability to speak using grade-appropriate English, with minimal second language acquisition support, in academic and social settings'),
    --reading
    (3, 1, 'Beginning ELLs have little or no ability to read and understand English used in academic and social contexts'),
    (3, 2, 'Intermediate ELLs have the ability to read and understand simple, high-frequency English used in routine academic and social contexts'),
    (3, 3, 'Advanced ELLs have the ability to read and understand, with second language acquisition support, grade-appropriate English used in academic and social contexts'),
    (3, 4, 'Advanced high ELLs have the ability to read and understand, with minimal second language acquisition support, grade-appropriate English used in academic and social contexts'),
    --writing
    (4, 1, 'Beginning ELLs have little or no ability to write in English'),
    (4, 2, 'Intermediate ELLs have the ability to write using simple English commonly heard in routine academic and social settings'),
    (4, 3, 'Advanced ELLs have the ability to write using grade-appropriate English, with second language acquisition support, in academic and social settings'),
    (4, 4, 'Advanced high ELLs have the ability to write using grade-appropriate English, with minimal second language acquisition support, in academic and social settings');

COMMIT;

BEGIN TRANSACTION
INSERT INTO elps.domain_lvl_attr
    (domain_level_id, grade_id, attr_type_id, attr)
VALUES

    -- Beginning level listening attributes
    (1, 16, 1, 'struggle to understand simple conversations and simple discussions even when the topics are familiar and the speaker uses linguistic supports such as visuals,slower speech and other verbal cues, and gestures'),
    (1, 16, 1, 'struggle to identify and distinguish individual words and phrases during social and instructional interactions that have not been intentionally modified for ELLs'),
    (1, 16, 1, 'may not seek clarification in English when failing to comprehend the English they hear; frequently remain silent, watching others for cues'),

    -- Intermediate level listening attributes
    (1, 16, 1, 'usually understand simple or routine directions, as well as short, simple conversations and short, simple discussions on familiar topics; when topics are unfamiliar, require extensive linguistic supports and adaptations such as visuals, slower speech and other verbal cues, simplified language, gestures, and preteaching to preview or build topic-related vocabulary'),
    (1, 16, 1, 'often identify and distinguish key words and phrases necessary to understand the general meaning during social and basic instructional interactions that have not been intentionally modified for ELLs'),
    (1, 16, 1, 'have the ability to seek clarification in English when failing to comprehend the English they hear by requiring/requesting the speaker to repeat, slow down, or rephrase speech'),

    -- Advanced level listening attributes
    (1, 16, 1, 'usually understand longer, more elaborated directions, conversations, and discussions on familiar and some unfamiliar topics, but sometimes need processing time and sometimes depend on visuals, verbal cues, and gestures to support understanding'),
    (1, 16, 1, 'understand most main points, most important details, and some implicit information during social and basic instructional interactions that have not been intentionally modified for ELLs'),
    (1, 16, 1, 'occasionally require/request the speaker to repeat, slow down, or rephrase to clarify the meaning of the English they hear'),

    -- Advanced high level listening attributes
    (1, 16, 1, 'understand longer, elaborated directions, conversations, and discussions on familiar and unfamiliar topics with occasional need for processing time and with little dependence on visuals, verbal cues, and gestures; some exceptions when complex academic or highly specialized language is used'),
    (1, 16, 1, 'understand main points, important details, and implicit information at a level nearly comparable to native English-speaking peers during social and instructional interactions'),
    (1, 16, 1, 'rarely require/request the speaker to repeat, slow down, or rephrase to clarify the meaning of the English they hear'),

    -- Beginning level speaking attributes
    (2, 16, 1, 'mainly speak using single words and short phrases consisting of recently practiced, memorized, or highly familiar material to get immediate needs met; may be hesitant to speak and often give up in their attempts to communicate'),
    (2, 16, 1, 'speak using a very limited bank of high-frequency, high-need, concrete vocabulary, including key words and expressions needed for basic communication in academic and social contexts'),
    (2, 16, 1, 'lack the knowledge of English grammar necessary to connect ideas and speak in sentences; can sometimes produce sentences using recently practiced, memorized, or highly familiar material'),
    (2, 16, 1, 'exhibit second language acquisition errors that may hinder overall communication, particularly when trying to convey information beyond memorized, practiced, or highly familiar material'),
    (2, 16, 1, 'typically use pronunciation that significantly inhibits communication'),

    -- Intermediate level speaking attributes
    (2, 16, 1, 'are able to express simple, original messages, speak using sentences, and participate in short conversations and classroom interactions'),
    (2, 16, 1, 'may hesitate frequently and for long periods to think about how to communicate desired meaning'),
    (2, 16, 1, 'speak simply using basic vocabulary needed in everyday social interactions and routine academic contexts; rarely have vocabulary to speak in detail'),
    (2, 16, 1, 'exhibit an emerging awareness of English grammar and speak using mostly simple sentence structures and simple tenses'),
    (2, 16, 1, 'are most comfortable speaking in present tense'),
    (2, 16, 1, 'exhibit second language acquisition errors that may hinder overall communication when trying to use complex or less familiar English'),
    (2, 16, 1, 'use pronunciation that can usually be understood by people accustomed to interacting with ELLs'),

    -- Advanced level speaking attributes
    (2, 16, 1, 'are able to participate comfortably in most conversations and academic discussions on familiar topics, with some pauses to restate, repeat, or search for words and phrases to clarify meaning'),
    (2, 16, 1, 'discuss familiar academic topics using content-based terms and common abstract vocabulary'),
    (2, 16, 1, 'can usually speak in some detail on familiar topics'),
    (2, 16, 1, 'have a grasp of basic grammar features, including a basic ability to narrate and describe in present, past, and future tenses'),
    (2, 16, 1, 'have an emerging ability to use complete sentences and complex grammar feature'),
    (2, 16, 1, 'make errors that interfere somewhat with communication when using complex grammar structures, long sentences, and less familiar words and expressions'),
    (2, 16, 1, 'may mispronounce words, but use pronunciation that can usually be understood by people not accustomed to interacting with ELLs'),

    -- Advanced high level speaking attributes
    (2, 16, 1, 'are able to participate in extended discussions on a variety of social and grade-appropriate academic topics with only occasional disruptions, hesitations, or pauses'),
    (2, 16, 1, 'communicate effectively using abstract and content-based vocabulary during classroom instructional tasks, with some exceptions when low-frequency or academically demanding vocabulary is needed'),
    (2, 16, 1, 'use many of the same idioms and colloquialisms as their native English-speaking peers'),
    (2, 16, 1, 'can use English grammar structures and complex sentences to narrate and describe at a level nearly comparable to native English-speaking peers'),
    (2, 16, 1, 'make few second language acquisition errors that interfere with overall communication'),
    (2, 16, 1, 'may mispronounce words, but rarely use pronunciation that interferes with overall communication'),

    -- Beginning level reading attributes
    --38
    (3, 14, 2, 'derive little or no meaning from grade-appropriate stories read aloud in English, unless the stories are:'),
    (3, 14, 1, 'begin to recognize and understand environmental print in English such as signs, labeled items, names of peers, and logos'),
    --40
    (3, 14, 2, 'have difficulty decoding most grade-appropriate English text because they:'),
    --41
    (3, 15, 2, 'read and understand the very limited recently practiced, memorized, or highly familiar English they have learned; vocabulary predominantly includes:'),
    (3, 15, 1, 'read slowly, word by word'),
    (3, 15, 1, 'have a very limited sense of English language structures'),
    (3, 15, 1, 'comprehend predominantly isolated familiar words and phrases; comprehend some sentences in highly routine contexts or recently practiced, highly familiar text'),
    (3, 15, 1, 'are highly dependent on visuals and prior knowledge to derive meaning from text in English'),
    (3, 15, 1, 'are able to apply reading comprehension skills in English only when reading texts written for this level'),

    -- Intermediate level reading attributes
    --47
    (3, 14, 2, 'demonstrate limited comprehension (key words and general meaning) of grade-appropriate stories read aloud in English, unless the stories include:'),
    (3, 14, 1, 'regularly recognize and understand common environmental print in English such as signs, labeled items, names of peers, logos'),
    --49
    (3, 14, 2, 'have difficulty decoding grade-appropriate English text because they:'),
    --50
    (3, 15, 2, 'read and understand English vocabulary on a somewhat wider range of topics and with increased depth; vocabulary predominantly includes'),
    (3, 15, 1, 'often read slowly and in short phrases; may re-read to clarify meaning'),
    (3, 15, 1, 'have a growing understanding of basic, routinely used English language structures'),
    (3, 15, 1, 'understand simple sentences in short, connected texts, but are dependent on visual cues, topic familiarity, prior knowledge, pretaught topic-related vocabulary, story predictability, and teacher/peer assistance to sustain comprehension'),
    (3, 15, 1, 'struggle to independently read and understand grade-level texts'),
    (3, 15, 1, 'are able to apply basic and some higher-order comprehension skills when reading texts that are linguistically accommodated and/or simplified for this level'),

    -- Advanced level reading attributes
    (3, 14, 1, 'demonstrate comprehension of most main points and most supporting ideas in grade-appropriate stories read aloud in English, although they may still depend on visual and linguistic supports to gain or confirm meaning'),
    (3, 14, 1, 'recognize some basic English vocabulary and high-frequency words in isolated print'),
    --58
    (3, 14, 2, 'with second language acquisition support, are able to decode most gradeappropriate English text because they'),
    --59
    (3, 15, 2, 'read and understand, with second language acquisition support, a variety of grade-appropriate English vocabulary used in social and academic contexts:'),
    (3, 15, 1, 'read longer phrases and simple sentences from familiar text with appropriate rate and speed'),
    (3, 15, 1, 'are developing skill in using their growing familiarity with English language structures to construct meaning of grade-appropriate text'),
    (3, 15, 1, 'are able to apply basic and higher-order comprehension skills when reading grade-appropriate text, but are still occasionally dependent on visuals, teacher/peer assistance, and other linguistically accommodated text features to determine or clarify meaning, particularly with unfamiliar topics'),

    -- Advanced high level reading attributes
    (3, 14, 1, 'demonstrate, with minimal second language acquisition support and at a level nearly comparable to native English-speaking peers, comprehension of main points and supporting ideas (explicit and implicit) in grade-appropriate stories read aloud in English'),
    (3, 14, 1, 'with some exceptions, recognize sight vocabulary and high-frequency words to a degree nearly comparable to that of native English-speaking peers'),
    (3, 14, 1, 'with minimal second language acquisition support, have an ability to decode and understand grade-appropriate English text at a level nearly comparable to native English-speaking peers'),
    (3, 15, 1, 'read and understand vocabulary at a level nearly comparable to that of their native English-speaking peers, with some exceptions when low-frequency or specialized vocabulary is used'),
    (3, 15, 1, 'generally read grade-appropriate, familiar text with appropriate rate, speed, intonation, and expression'),
    (3, 15, 1, 'are able to, at a level nearly comparable to native English-speaking peers, use their familiarity with English language structures to construct meaning of grade-appropriate text'),
    (3, 15, 1, 'are able to apply, with minimal second language acquisition support and at a level nearly comparable to native English-speaking peers, basic and higher-order comprehension skills when reading grade-appropriate text'),

    -- Beginning level writing attributes
    (4, 1, 1, 'are unable to use English to explain self-generated writing such as stories they have created or other personal expressions, including emergent forms of writing (pictures, letter-like forms, mock words, scribbling, etc.)'),
    (4, 1, 1, 'know too little English to participate meaningfully in grade-appropriate shared writing activities using the English language'),
    (4, 1, 1, 'cannot express themselves meaningfully in self-generated, connected written text in English beyond the level of high-frequency, concrete words, phrases, or short sentences that have been recently practiced and/or memorized'),
    (4, 1, 1, 'may demonstrate little or no awareness of English print conventions'),
    (4, 1, 1, 'have little or no ability to use the English language to express ideas in writing and engage meaningfully in grade-appropriate writing assignments in content area instruction'),
    (4, 1, 1, 'lack the English necessary to develop or demonstrate elements of grade-appropriate writing such as focus and coherence, conventions, organization, voice, and development of ideas in English'),
    --76
    (4, 1, 2, 'exhibit writing features typical at this level, including:'),

    -- Intermediate level writing attributes
    (4, 2, 1, 'know enough English to explain briefly and simply self-generated writing, including emergent forms of writing, as long as the topic is highly familiar and concrete and requires very high-frequency English'),
    (4, 2, 1, 'can participate meaningfully in grade-appropriate shared writing activities using the English language only when the writing topic is highly familiar and concrete and requires very high-frequency English'),
    (4, 2, 1, 'express themselves meaningfully in self-generated, connected written text in English when their writing is limited to short sentences featuring simple, concrete English used frequently in class'),
    (4, 2, 1, 'frequently exhibit features of their primary language when writing in English such as primary language words, spelling patterns, word order, and literal translating'),
    (4, 2, 1, 'have a limited ability to use the English language to express ideas in writing and engage meaningfully in grade-appropriate writing assignments in content area instruction'),
    (4, 2, 1, 'are limited in their ability to develop or demonstrate elements of gradeappropriate writing in English; communicate best when topics are highly familiar and concrete, and require simple, high-frequency English'),
    --83
    (4, 2, 2, 'exhibit writing features typical at this level, including:'),

    -- Advanced level writing attributes
    (4, 3, 1, 'use predominantly grade-appropriate English to explain, in some detail, most self-generated writing, including emergent forms of writing'),
    (4, 3, 1, 'can participate meaningfully, with second language acquisition support, in most grade-appropriate shared writing activities using the English language;'),
    (4, 3, 1, 'These students, although second language acquisition support is needed, have an emerging ability to express themselves in self-generated, connected written text in English in a grade-appropriate manner'),
    (4, 3, 1, 'occasionally exhibit second language acquisition errors when writing in English'),
    (4, 3, 1, 'are able to use the English language, with second language acquisition support, to express ideas in writing and engage meaningfully in grade-appropriate writing assignments in content area instruction'),
    (4, 3, 1, 'know enough English to be able to develop or demonstrate elements of grade-appropriate writing in English, although second language acquisition support is particularly needed when topics are abstract, academically challenging, or unfamiliar'),
    --90
    (4, 3, 2, 'exhibit writing features typical at this level, including:'),

    -- Advanced high level writing attributes
    (4, 4, 1, 'use English at a level of complexity and detail nearly comparable to that of native English-speaking peers when explaining self-generated writing, including emergent forms of writing'),
    (4, 4, 1, 'can participate meaningfully in most grade-appropriate shared writing activities using the English language'),
    (4, 4, 1, 'although minimal second language acquisition support may be needed, express themselves in self-generated, connected written text in English in a manner nearly comparable to their native English-speaking peers'),
    (4, 4, 1, 'are able to use the English language, with minimal second language acquisition support, to express ideas in writing and engage meaningfully in grade-appropriate writing assignments in content area instruction'),
    (4, 4, 1, 'know enough English to be able to develop or demonstrate, with minimal second language acquisition support, elements of grade-appropriate writing in English'),
    --96
    (4, 4, 2, 'exhibit writing features typical at this level, including:');

COMMIT;


BEGIN TRANSACTION
INSERT INTO elps.domain_lvl_attr_item
    (domain_lvl_attr_id, item)
VALUES
    -- Beginning level reading attribute items
    (38, 'read in short chunks'),
    (38, 'controlled to include the little English they know such as language that is high frequency, concrete, and recently practiced'),
    (38, 'accompanied by ample visual supports such as illustrations, gestures, pantomime, and objects and by linguistic supports such as careful enunciation and slower speech'),

    (40, 'understand the meaning of very few words in English'),
    (40, 'struggle significantly with sounds in spoken English words and with sound-symbol relationships due to differences between their primary language and English'),

    (41, 'environmental print'),
    (41, 'some very high-frequency words'),
    (41, 'concrete words that can be represented by pictures'),

    -- Intermediate level reading attribute items
    (47, 'predictable story lines'),
    (47, 'highly familiar topics'),
    (47, 'primarily high-frequency, concrete vocabulary'),
    (47, 'short, simple sentences'),
    (47, 'visual and linguistic supports'),

    (49, 'understand the meaning of only those English words they hear frequently'),
    (49, 'struggle with some sounds in English words and some sound-symbol relationships due to differences between their primary language and English'),

    (50, 'everyday oral language'),
    (50, 'literal meanings of common words'),
    (50, 'routine academic language and terms'),
    (50, 'commonly used abstract language such as terms used to describe basic feelings'),

    -- Advanced level reading attribute items
    (58, 'understand the meaning of most grade-appropriate English words'),
    (58, 'have little difficulty with English sounds and sound-symbol relationships that result from differences between their primary language and English'),

    (59, 'with second language acquisition support, read and understand gradeappropriate concrete and abstract vocabulary, but have difficulty with less commonly encountered words'),
    (59, 'demonstrate an emerging ability to understand words and phrases beyond their literal meaning'),
    (59, 'understand multiple meanings of commonly used words'),

    -- Beginning level writing attribute items
    (76, 'ability to label, list, and copy'),
    (76, 'high-frequency words/phrases and short, simple sentences (or even short paragraphs) based primarily on recently practiced, memorized, or highly familiar material; this type of writing may be quite accurate'),
    (76, 'present tense used primarily'),
    (76, 'frequent primary language features (spelling patterns, word order, literal translations, and words from the students primary language) and other errors associated with second language acquisition may significantly hinder or prevent understanding, even for individuals accustomed to the writing of ELLs'),

    -- Intermediate level writing attribute items
    (83, 'simple, original messages consisting of short, simple sentences; frequent inaccuracies occur when creating or taking risks beyond familiar English'),
    (83, 'high-frequency vocabulary; academic writing often has an oral tone'),
    (83, 'loosely connected text with limited use of cohesive devices or repetitive use, which may cause gaps in meaning'),
    (83, 'repetition of ideas due to lack of vocabulary and language structures'),
    (83, 'present tense used most accurately; simple future and past tenses, if attempted, are used inconsistently or with frequent inaccuracies'),
    (83, 'undetailed descriptions, explanations, and narrations; difficulty expressing abstract ideas'),
    (83, 'primary language features and errors associated with second language acquisition may be frequent'),
    (83, 'some writing may be understood only by individuals accustomed to the writing of ELLs; parts of the writing may be hard to understand even for individuals accustomed to ELL writing'),

    --Advanced level writing attribute items
    (90, 'grasp of basic verbs, tenses, grammar features, and sentence patterns; partial grasp of more complex verbs, tenses, grammar features, and sentence patterns'),
    (90, 'emerging grade-appropriate vocabulary; academic writing has a more academic tone'),
    (90, 'use of a variety of common cohesive devices, although some redundancy may occur'),
    (90, 'narrations, explanations, and descriptions developed in some detail with emerging clarity; quality or quantity declines when abstract ideas are expressed, academic demands are high, or low-frequency vocabulary is required'),
    (90, 'occasional second language acquisition errors'),
    (90, 'communications are usually understood by individuals not accustomed to the writing of ELLs'),

    -- Advanced high level writing attribute items
    (96, 'nearly comparable to writing of native English-speaking peers in clarity and precision with regard to English vocabulary and language structures, with occasional exceptions when writing about academically complex ideas, abstract ideas, or topics requiring low-frequency vocabulary'),
    (96, 'occasional difficulty with naturalness of phrasing and expression'),
    (96, 'errors associated with second language acquisition are minor and usually limited to low-frequency words and structures; errors rarely interfere with communication');

COMMIT;

-- BEGIN TRANSACTION
-- INSERT INTO elps.strategies_objectives 
-- (strategy_mod_id, domain_objective_id)
-- VALUES 
-- ()
-- COMMIT;