
const LANGUAGES = { ENGLISH: "EN", SPANISH: "ESP" }
const LESSON_YEAR = "2024";

const TEMPLATE_NAMES = {
  EDU_ICON_LEGEND: "edu_icon_lgnd",
  EDU_COPYRIGHT: "edu_copyright",
  EDU_LO_DOL: "edu_lo_dol",
  EDU_ICLO: "edu_iclo",
  EDU_COGNATES: "edu_cognates",
  EDU_REVIEW: "edu_review"
}
const FULL_SUPPORTS_TEMPLATE_K_2_ID = "1-vXt6keTTeSkQnRhaqLxbXmy5P5QdQrbItPUIz1OaZw"
const FULL_SUPPORTS_TEMPLATES_3_5_ID = "1cpWUdXikkXHIcpYF3lEGLCrbk8F5QvBh2d5_vg_m058"
const SPANISH_SUPPORTS_TEMPLATES_ID = "1T0StzTxusglhkVRhaVekGGrYmhi_BAxRd1e6NLXGtUc"

const FULL_SUPPORTS_K_2 = {
  fileId: FULL_SUPPORTS_TEMPLATE_K_2_ID,
  grades: ["K", "1", "2"],
  templates: [
    TEMPLATE_NAMES.EDU_ICON_LEGEND,
    TEMPLATE_NAMES.EDU_COPYRIGHT,
    TEMPLATE_NAMES.EDU_ICLO,
    TEMPLATE_NAMES.EDU_LO_DOL,
    TEMPLATE_NAMES.EDU_COGNATES,
    TEMPLATE_NAMES.EDU_REVIEW]
}

const FULL_SUPPORTS_3_5 = {
  fileId: FULL_SUPPORTS_TEMPLATE_K_2_ID,
  grades: ["3", "4", "5"],
  templates: [
    TEMPLATE_NAMES.EDU_ICON_LEGEND,
    TEMPLATE_NAMES.EDU_COPYRIGHT,
    TEMPLATE_NAMES.EDU_ICLO,
    TEMPLATE_NAMES.EDU_LO_DOL,
    TEMPLATE_NAMES.EDU_COGNATES,
    TEMPLATE_NAMES.EDU_REVIEW]
}

const SPANISH_SUPPORTS = {
  fileId: SPANISH_SUPPORTS_TEMPLATES_ID,
  grades: ["ALL"],
  templates: [
    TEMPLATE_NAMES.EDU_ICON_LEGEND,
    TEMPLATE_NAMES.EDU_COPYRIGHT,
    TEMPLATE_NAMES.EDU_ICLO,
    TEMPLATE_NAMES.EDU_LO_DOL,
    TEMPLATE_NAMES.EDU_REVIEW]
}

const EDU_SUPPORTS = [FULL_SUPPORTS_K_2, FULL_SUPPORTS_3_5]

const ICON_DROP_INDEX = 2;
const ICLO_INDEX_OFFSET = 3;
const ELPS_STRATEGY_OFFSET = 2;
const LO_DOL_INDEX_OFFSET = 2;
const COGNATES_INDEX_OFFET = 3;


function getElpsByDate(lessonDate){

  var apiUrl = 'https://revisa-api.azurewebsites.net/language_supports/iclo' + '?delivery_date=' + lessonDate;
  var headers = {
    'Content-Type': 'application/json',
  };

  // Send the file data to the API
  var options = {
    'method': 'get',
    'headers': headers,
  };

  // Make the HTTP request
  var response = UrlFetchApp.fetch(apiUrl, options);

  // Handle the API response
  var responseData = response.getContentText();
  var json = JSON.parse(responseData);

  return json
}








const READY_TO_PROCESS_FOLDER_ID = "10eShvnJw8zdPjtdpZeu3f6LdLZhBwMVB";
const WIP_FOLDER_ID = "1T5CNB88bzY_46QFIVdDVXWCA5iV18Wm6";
const PROCESSED_ORIGINALS_FOLDER_ID = "1vZzmbr6uCt_LGhco0lt-dg3Zz0UgQTkc";
const OUTBOX_FOLDER_ID = "1MqS9HpcK7Uew-ARQeEhhPXe4IuQJenvJ";
const RETRY_FOLDER_ID = "1hugeUqR4ZfpOitgapHzN8Xb0oVefAhHD";

// orchestrator which reads Slides files imported to the Ready To Process Folder, 
// applying templates and ELPS supports as appropriate
// save the content of this lesson
// successfully processed slides will then be linked to a newly created ClickUp task
// client spreadsheet will be updated to reflect successful import
// returns metadata on each processed slide
function processImportedSlides() {

  //check ready-to-process folder for imported lessons
  const readyToProcessFiles = DriveApp.getFolderById(READY_TO_PROCESS_FOLDER_ID).getFiles();
  var processedFilesData = [];

  while (readyToProcessFiles.hasNext()) {
    var fileMaster = readyToProcessFiles.next();

    if (fileMaster.getMimeType() === MimeType.GOOGLE_SLIDES) {
      var fileCopy = fileMaster.makeCopy();

      // make sure to continue processing files, even if one slide deck has issues
      try {
        var result;
        // metadata is gatherered thru the import process and passed along to working copy
        const fileMetadata = JSON.parse(fileMaster.getDescription());
        fileCopy.setDescription(JSON.stringify(fileMetadata));


        //process the working file 
        applySupports(fileCopy.getId(), JSON.parse(fileCopy.getDescription()));

        fileCopy.moveTo(DriveApp.getFolderById(WIP_FOLDER_ID));
        fileMaster(moveTo(DriveApp.getFolderById(PROCESSED_ORIGINALS_FOLDER_ID)));

        

        // create clickup task
        // be sure to get latest file description

        // update spreadsheet
        // be sure to get latest file description





        // add working file metadata for reporting
        processedFilesData.push(JSON.parse(fileCopy.getDescription()));
      } catch (e) {
        Logger.log(e)
        // if an error occurs, trash the working copy and move the master to retry folder
        fileCopy.setTrashed(true);
        fileMaster.moveTo(DriveApp.getFolderById(RETRY_FOLDER_ID));
        continue;
      }
    }

  }
  return processedFilesData;
}



const ELPS_STRATEGY_SLIDES_FOLDER_ID = ""
function applySupports(lessonFileId, lessonMetadata) {

  var lessonSlideFile = SlidesApp.openById(lessonFileId);

  if (lessonMetadata.info.language.toUpperCase() === LANGUAGES.ENGLISH) {
    var supportSlides = applyEnglishLanguageSupportSlides(lessonSlideFile, lessonMetadata);
    lessonSlideFile.saveAndClose();
    applyEnglishLanguageSupportContent(lessonSlideFile, lessonMetadata, supportSlides)
  }
  else if (lessonMetadata.info.language.toUpperCase() === LANGUAGES.SPANISH) {
    applySpanishLanguageSupports(lessonSlideFile, lessonMetadata)
  }

}

function applyEnglishLanguageSupportSlides(lessonSlideFile, lessonMetadata) {
  Logger.log("ENGLISH")

  //load in our templates
  var templates = EDU_SUPPORTS.filter(es => es.grades.includes(lessonMetadata.info.grade))[0];
  templates["templateSlideFile"] = SlidesApp.openById(templates.fileId);

  const elps = getElpsByDate((lessonMetadata.info.delivery_date + "." + LESSON_YEAR))
  // 1. Icon Slide
  const iconLgndSlide = lessonSlideFile.insertSlide(ICON_DROP_INDEX, templates.templateSlideFile.getSlideById(TEMPLATE_NAMES.EDU_ICON_LEGEND))

  // 2. ICLO (CLC) Slide
  var lessonSlides = lessonSlideFile.getSlides();
  var titleSlideIndexes = [];
  lessonSlides.forEach((lessonSlide, slideIndex) => {
    const layoutName = lessonSlide.getLayout().getLayoutName()
    if (layoutName.toUpperCase() === "TITLE SLIDE") {
      titleSlideIndexes.push(slideIndex)
    }
  })
  titleSlideIndexes.sort((i, j) => i - j) // sort ascending
  const icloSlide = lessonSlideFile.insertSlide(titleSlideIndexes[1] - 2, templates.templateSlideFile.getSlideById(TEMPLATE_NAMES.EDU_ICLO))

  // 3. ELPS Strategy
  const elpsStrategyTemplate = SlidesApp.openById(elps.elps_strategy_file_id).getSlides()[0] // each elps strategy slide deck has one slide
  var elpsStrategySlide = lessonSlideFile.insertSlide(titleSlideIndexes[1] - 1, elpsStrategyTemplate);

  // 4. LO/DOL
  // this one is unique in that we need to scrape text from the original slide, then delete the original
  const loDolOriginalSlideIndex = getLoDolSlideIndex(lessonSlides);
  var loDolOriginalSlide = lessonSlides[loDolOriginalSlideIndex];
  const newLoDolSlide = lessonSlideFile.insertSlide(loDolOriginalSlideIndex + 1, templates.templateSlideFile.getSlideById(TEMPLATE_NAMES.EDU_LO_DOL));

  // 5. Cognates
  const cognatesSlide = lessonSlideFile.insertSlide(loDolOriginalSlideIndex + 3, templates.templateSlideFile.getSlideById(TEMPLATE_NAMES.EDU_COGNATES))

  // 6. Review
  // We base this on the Title Slide slides found in the deck

  var reviewSlide;
  if (titleSlideIndexes.length > 2) {
    // if there are more than 2 title slides, it means there are additional sections after the main lesson
    // we shouldn't place the review slide at the end in this case
    reviewSlide = lessonSlideFile.insertSlide(titleSlideIndexes[2] - 1, templates.templateSlideFile.getSlideById(TEMPLATE_NAMES.EDU_REVIEW))
  }
  else {
    // if there are only 2 title slides, we can safely place the slide at the end of the deck
    reviewSlide = lessonSlideFile.insertSlide(lessonSlides.length - 1, templates.templateSlideFile.getSlideById(TEMPLATE_NAMES.EDU_REVIEW))
  }

  return {
    iconLgnd: iconLgndSlide.getObjectId(),
    iclo: icloSlide.getObjectId(),
    elpsStrategy: elpsStrategySlide.getObjectId(),
    loDolOriginal: loDolOriginalSlide.getObjectId(),
    newLoDolx: newLoDolSlide.getObjectId(),
    cognates: cognatesSlide.getObjectId(),
    review: reviewSlide.getObjectId()
  }

}


function applyEnglishLanguageSupportContent() {

}

function applySpanishLanguageSupports(lessonSlideFile) {
  Logger.log("SPANISH")
}

// attempts to find LO/DOL Slide and returns its index. If it's not found, returns -1
function getLoDolSlideIndex(lessonSlides) {
  // we need to extract any text found on the original and apply it to the EDU LO/DOL template
  lessonSlides.forEach((sl, slideIndex) => {
    sl.getPageElements().forEach(element => {
      if (element.getPageElementType() === SlidesApp.PageElementType.SHAPE) {
        const elementText = element.asShape().getText().asString();
        Logger.log(elementText);
        if ((elementText.includes("LO:") && elementText.includes("DOL:"))
          || (elementText.includes("Objetivo de aprendizaje:") && elementText.includes("Demostración de aprendizaje:"))) {
          return slideIndex;
        }
      }
    })
  })
  return -1;
}



