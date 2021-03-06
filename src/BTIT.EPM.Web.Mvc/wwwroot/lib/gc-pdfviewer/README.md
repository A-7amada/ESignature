# GrapeCity Documents PDF Viewer

#### [日本語](#japanese)

A full-featured JavaScript PDF viewer and editor that comes with [GrapeCity Documents for PDF](https://www.grapecity.com/documents-api-pdf).

__GrapeCity Documents PDF Viewer__ (__GcDocs PDF Viewer__, __GcPdfViewer__) is a fast, modern JavaScript based PDF viewer and editor that runs in all major browsers.
The viewer can be used as a cross platform solution to view (or modify - see _Support API_ below) PDF documents on Windows, MAC, Linux, iOS and Android devices.
GcPdfViewer is included in [GrapeCity Documents for PDF (GcPdf)](https://www.grapecity.com/documents-api-pdf) - a feature-rich cross-platform PDF API library for .NET Core.

[_Support API_](#support_api) is a server-side NuGet package
[GrapeCity.Documents.Pdf.ViewerSupportApi](https://www.nuget.org/packages/GrapeCity.Documents.Pdf.ViewerSupportApi/)
that allows you to easily set up an ASP.&#8203;NET Core or a Web Forms server that employs GcPdf
to add PDF editing abilities to GcPdfViewer.
When connected to a _Support API_ server, GcPdfViewer becomes a powerful PDF editor that can be used
to edit existing or create new PDF documents, fill and save PDF forms,
remove (redact) sensitive content, add or edit annotations and AcroForm fields, and more.

GcPdfViewer provides a rich client side JavaScript object model, see __docs/index.html__ for the client API documentation.

Product highlights:

- Works in all modern browsers, including IE11, Edge, Chrome, FireFox, Opera, Safari
- When connected to _GcPdf_ on the server via _Support API_, provides:
  - Customizable and mobile-friendly form filler
  - Real-time collaboration mode
  - Annotation and form editors
  - Quick edits using secondary toolbars
  - PDF redaction
  - Signature verification
  - Other editing features
- Works with frameworks such as React, Preact, Angular
- Supports form filling; filled forms can be printed or form data can be submitted to the server
- Supports XFA (XML Forms Architecture) forms
- Provides caret for text selection/copy, including vertical and RTL texts
- Includes thumbnails, text search, outline, attachments, articles, layers and structure tags panels
- Allows opening PDF files from local disks
- Supports annotations including text, free text, rich text etc.
- Supports redact annotations (including appearance streams), allows user to hide or show redacts
- Supports sound annotations
- Allows rotating and printing the rotated document
- Supports article threads navigation
- Fully supports file attachments (both attachment annotations and document level attachments)
- Comes with several themes, new custom themes can be added
- Supports external CMaps
- ...and more.

## [3.1.4] - 18-May-2022
### Changed
- Documentation files updated.

## [3.1.3] - 29-Apr-2022
### Fixed
- [Regression] A checkbox with an attached JavaScript automatically unchecks when checked. (DOC-4244)
- [Editor] Cannot draw horizontal or vertical lines using ink annotation tool. (DOC-4245)
- [Editor] Undo works incorrectly when undoing rotating a stamp. (DOC-4237)
- [Editor] Incorrect appearance of stamp annotations after rotation. (DOC-4208, DOC-4225, DOC-4227)
- In some scenarios moving the mouse resizes the left panel. (DOC-4238)
### Changed
- Updated Japanese localization strings.
### Added
- Added resize handles to panels. (DOC-3171)

## [3.1.2] - 12-Apr-2022
### Fixed
- [Editor] Annotations and fields are added in incorrect orientation when the document is rotated. (DOC-3260)
- [Android] Zooming using pinch does not work. (DOC-3424)
- [Collaboration] Sharing a multi-page PDF is not working correctly. (DOC-4143)
- [Collaboration] The file name is incorrect in the "Manage Access" dialog. (DOC-4144)
- [JavaScript actions] Viewer properties are undefined in JavaScript actions. (DOC-4154)
- In some cases the visibility state of layers is incorrect after saving a PDF. (DOC-4067)
### Changed
- Do not show the version mismatch warning if the connected SupporApi has version 0.0.0.0 (was built from sources).
### Added
- New method invalidate: ensures that all visual child elements of the viewer are properly updated for layout.
- New property requiredSupportApiVersion: gets the required version of SupportApi that is compatible with the current version of GcPdfViewer.
- New property supportApiVersion: gets the connected version of SupportApi, if available.
- New property gcPdfVersion: gets the version of GcPdf library used by the connected SupportApi, if available.
- [Editor] Ability to rotate stamp and free text annotations using rotation handles. 
  Pressing the Shift key snaps the rotation angle to a multiple of 90 degrees. (DOC-4058)
- [XFA forms] Added support for print, submit, reset, JavaScript actions, links. (DOC-3869)
- [Editor] Ability to persist the visibility state of optional content groups (layers) when saving the PDF. (DOC-3652)
- [Editor] Ability to change a widget's content orientation using the orientation property. (DOC-3260)
- [Editor] Sticky behavior for toolbar buttons: a button remains pressed after the editing operation is complete. (DOC-3913)\
  Added stickyBehavior setting to toolbarLayout: an array with button keys that will have sticky behavior. 
  Note that only annotation and form editor toolbar buttons can be made sticky.\
  The complete list of buttons that can be made sticky: 'edit-sign-tool', 'edit-text', 'edit-free-text', 'edit-ink', 'edit-square', 'edit-circle', 'edit-line', 'edit-polyline', 'edit-polygon', 'edit-stamp', 'edit-file-attachment',
  'edit-sound', 'edit-link', 'edit-redact', 'edit-widget-tx-field', 'edit-widget-tx-password',
  'edit-widget-tx-text-area', 'edit-widget-btn-checkbox', 'edit-widget-btn-radio',
  'edit-widget-btn-push', 'edit-widget-ch-combo', 'edit-widget-ch-list-box', 'edit-widget-tx-comb',
  'edit-widget-btn-submit', 'edit-widget-btn-reset', 'edit-erase-field'
```javascript
  // Example: make square, circle, line and redact buttons sticky:
  viewer.toolbarLayout.stickyBehavior = ["edit-square", "edit-circle", "edit-line", "edit-redact"];
```
- Option fieldsAppearance: added the ability to specify button fields render type. (DOC-3537)
```javascript  
  // Example 1: Use platform-native styling for push buttons.
  var viewer = new GcPdfViewer("#root", { fieldsAppearance: { pushButton: "Web" } });
  // Example 2: Use predefined appearance stream for push buttons:
  var viewer = new GcPdfViewer("#root", { fieldsAppearance: { pushButton: "Predefined" } });
```
- Ability to programmatically hide the left sidebar. (DOC-3981)
```javascript
  // Hide sidebar:
  viewer.toggleSidebar(false);
  // Alternative variant:
  viewer.leftSidebar.hide();
```
- Ability to programmatically hide or show the toolbar. (DOC-3981)
```javascript
  // Example:
  viewer.toggleToolbar(false);
```
- Option hideAnnotationPopups: use this option to hide all annotation popups. (DOC-3981)
```javascript
  // Example:
  var viewer = new GcPdfViewer("#root", { hideAnnotationPopups: true });
```
- Ability to close the currently loaded document. (DOC-3981)
```javascript
  // Example:
  await viewer.close();
```
- New events: onBeforeAddAnnotation, onAfterAddAnnotation, onBeforeUpdateAnnotation, onAfterUpdateAnnotation,
  onBeforeRemoveAnnotation, onAfterRemoveAnnotation. (DOC-3981)
```javascript
  // Example:
  viewer.onBeforeAddAnnotation.register(function(args) { console.log(args); });
```
  Events BeforeAddAnnotation, BeforeUpdateAnnotation and BeforeRemoveAnnotation are cancelable.
```javascript
  // Examples of canceling events:
  viewer.getEvent("BeforeAddAnnotation").register(function(args) { args.cancel = true; });
  viewer.getEvent("BeforeUpdateAnnotation").register(function(args) {
    args.cancel = true;
    viewer.repaint();
  });
```
- Ability to listen to and trigger custom events. (DOC-3981)
```javascript
  // Example: listen to CustomEvent:
  viewer.getEvent("CustomEvent").register(function(args) {
    console.log(args);
  });
  // Example: trigger CustomEvent:
  viewer.triggerEvent("CustomEvent", { arg1: 1, arg2: 2});
```

## [3.0.21] - 22-Mar-2022
### Fixed
- [Regression] [Form Editor] Unable to move form fields using the arrow keys. (DOC-4115)
- [Form Editor] Text cursor moves when an annotation is moved using the arrow keys. (DOC-4123)
- [Regression] Missing "token" parameter when requesting a stamp image using GET method. (DOC-4080)
- In some cases the visibility state of layers is incorrect after loading or saving a PDF. (DOC-4068)

## [3.0.20] - 03-Mar-2022
### Fixed
- When renderInteractiveForms is false, checkbox values are not displayed. (DOC-4022)
- Highlight on the search text disappears when the document is zoomed in or out. (DOC-4028)
- [Editor] Custom image stamps are gone when the viewer is closed and recreated. (DOC-4023)

## [3.0.19] - 25-Jan-2022
### Added
- Added the ability to specify authentication or other HTTP headers in the open() method. (DOC-3998)
```javascript
  // Example: specify basic access authentication and custom headers:
  viewer.open("http://example.com//pdfs/GetPdf?file=HelloWorld.pdf", { 
    headers: { 
      "Authorization": "Basic " + btoa(unescape(encodeURIComponent("USERNAME:PASSWORD"))),
      "CustomHeader": "Custom header value"
    }
  });
```
 - Added the ability to specify authentication or other HTTP headers in SupportApi requests. (DOC-3998)
```javascript
  // Example: specify basic access authentication and custom headers:
  const viewer = new GcPdfViewer("#viewer", {
    supportApi: {
      apiUrl: "192.168.0.1/support-api",
      requestInit: { 
        headers: {
          "Authorization": "Basic " + btoa(unescape(encodeURIComponent("USERNAME:PASSWORD"))),
          "CustomHeader": "Custom header value"
        }
      }
    }
  });
```
### Fixed
- [Editor] Text added in the free text annotation editor disappears on resizing the annotation. (DOC-3988)

## [3.0.18] - 24-Jan-2022
### Fixed
- Undo history should be cleared by the close() method. (DOC-3989)

## [3.0.17] - 21-Jan-2022
### Added
- [Form Editor] "Editable" property added to combo boxes. (DOC-3947)
- [Form Editor] New values added to "Tab order" property: "Annotations" and "Widgets". (DOC-3668)
- Added ability to close the current document. (DOC-3981)
```javascript
  // Usage example:
  await viewer.close();
```
### Fixed
- Incorrect tab cycle for an editable combo box. (DOC-3967)
- Tab order differs from Adobe Acrobat Reader in some cases. (DOC-3668)
- [Form Editor] Cannot reset the "Tab order" property to "Not specified". (DOC-3968)
- [Editor] The Backspace key does not work in the free text annotation editor. (DOC-3983)
- [Editor] If a new page is inserted and the document is saved, the tab order of following pages is incorrect. (DOC-3985)

## [3.0.13] - 22-Dec-2021
### Added
- Added support for editable combo boxes. (DOC-3947)
- [XFA forms] Added the ability to select/copy text content. (DOC-3917)
- [Editor] Remember last used editor values. (DOC-3925)\
  New settings added to the "editorDefaults" option: "rememberLastValues" and "lastValueKeys".
  If "rememberLastValues" is set to true or undefined, the last used property values will be used as default values for new annotations.
  "lastValueKeys" specifies which properties will be remembered.
```javascript  
  // The default value of lastValueKeys:
  ["appearanceColor", "borderStyle", "color", "interiorColor", "backgroundColor", "borderColor", "opacity", "textAlignment", "printableFlag", "open",
  "lineStart", "lineEnd", "markBorderColor", "markFillColor", "overlayFillColor", "overlayText", "overlayTextJustification", "newWindow", "calloutLineEnd", "fontSize",
  "fontName", "name", "readOnly", "required"]
```
```javascript  
  // Example: turn remembering last used values off:
  var viewer = new GcPdfViewer("#root", { editorDefaults: { rememberLastValues: false } });
```
```javascript
  // Example: remember only the borderStyle property:
  var viewer = new GcPdfViewer("#root", { editorDefaults: { rememberLastValues: true, lastValueKeys: ["borderStyle"] } });
```
- [Android] Added support for zooming using pinch gesture. (DOC-3424, ARD-2508)
### Changed
- Japanese localization strings updated.
### Fixed
- [iPad] PDF will not render after max zooming when iPad is running in Desktop mode. (DOC-3765)
- [iOS][Android] The main toolbar collapses when the secondary toolbar is clicked. (DOC-3843)
- [Editor] Method showSecondToolbar does not activate editor mode correctly. (DOC-3892)
- [Editor] Incorrect undo / redo behavior when editing ink annotations. (DOC-3924)
- Incorrect zoom controls behavior. (DOC-3929)

## [3.0.10] - 25-Nov-2021
### Changed
- __Breaking change__: All public APIs that used page numbers now use zero-based page indices. (DOC-3536)
- PDF.js library was updated from v2.0.943 to v2.10.377, see [PDF.js Release Notes](https://github.com/mozilla/pdf.js/releases).
- Method goToPageNumber deprecated, use goToPage method or pageIndex property instead. (DOC-3536)
- By default, radio buttons and checkboxes now do not use predefined appearances from the PDF. 
  Use the fieldsAppearance option to revert to the old behavior:
```javascript  
  var viewer = new GcPdfViewer("#root", { fieldsAppearance: { radioButton: "Predefined", checkBoxButton: "Predefined" } });
```
- [SupportApi client] The ping() method has been deprecated and is no longer used; instead, the serverVersion() method is used.
- Properties SubmitForm/ResetForm renamed to submitForm/resetForm. (DOC-3379)
### Added
- Added ability to edit document without switching to Annotation Editor or Form Editor modes. (DOC-3169)
- Added support for second horizontal toolbar.
- Added secondary editing toolbars to the main viewer toolbar: "Text tools", "Draw tools", "Attachments and stamps", "Form tools", "Page tools".\
  Use secondToolbarLayout to control which toolbars are enabled.
  ```javascript
  // Example: specify custom second toolbar layout:
  viewer.secondToolbarLayout = { "text-tools": ['edit-text', 'edit-free-text'] };
  ```
- Added API to display a custom second toolbar. (DOC-3170)
  ```javascript
  // Example: create custom second toolbar with key "custom-toolbar-key":
  var React = viewer.getType("React");
  var toolbarControls = [React.createElement("label", null, "Custom toolbar"),
  React.createElement("button", { onClick: () => { alert("Execute action."); }, title: "Action title" }, "Action")];
  // Register custom second toolbar for key "custom-toolbar-key":
  viewer.options.secondToolbar = {
   render: function(toolbarKey) {
     if(toolbarKey === "custom-toolbar-key")
       return toolbarControls;
     return null;
   }
  };
  // Show custom second toolbar:
  viewer.showSecondToolbar("custom-toolbar-key");
  ```
- Added Light and Dark themes. (DOC-3169)
- Support page content accessibility for tagged PDFs containing logical structure information for screen readers.
- Added ability to show the structure tree of tagged PDFs. (DOC-3131)
  ```javascript
  // Use addStructureTreePanel method to add the appropriate panel:
  const viewer = new GcPdfViewer(selector);
  viewer.addStructureTreePanel();
  await viewer.open("sample.pdf");
  ```
  ```javascript
  // Use structureTree to access available structure tree data:
  const viewer = new GcPdfViewer(selector);
  await viewer.open("sample.pdf");
  const structureTree = await viewer.structureTree;
  if(structureTree) {
   console.log(structureTree);
  }
  ```
- Added goToPage method: navigate to the page with a specified 0-based index.
  ```javascript
  // Example: go to the first page:
  viewer.goToPage(0);
  ```
- Added option maxCanvasPixels: maximum supported canvas size in pixels, i.e. width * height.  Undefined or -1 means no limit.
  If the canvas scaling exceeds maxCanvasPixels, the CSS scaling is used instead of re-rendering the page to the canvas. (DOC-3765)
- Added the ability to use GET method to submit a form.
  ```csharp
  // Using GcPdf to create an ActionSubmitForm to submit a form using the GET method:
  var actionSubmit = new ActionSubmitForm("/");
  actionSubmit.SubmitFormat = ActionSubmitForm.SubmitFormatType.HtmlForm;
  actionSubmit.HtmlFormFormat = ActionSubmitForm.HtmlFormFormatFlags.GetMethod;
  var btnSubmit = new PushButtonField();
  btnSubmit.Widget.Rect = new RectangleF(50, 100, 100, 50);
  btnSubmit.Widget.ButtonAppearance.Caption = "Submit";
  btnSubmit.Widget.Page = page;
  btnSubmit.Widget.Events.Activate = actionSubmit;
  ```
- Action reset: added support for FieldNames, ExcludeSpecifiedFields and Next properties. (DOC-3379)
- Added new option fieldsAppearance - specifies how form fields are rendered. (DOC-3537)\
  Use this option to customize rendering of form fields.
  Available appearance rendering types:
  * "Custom" - Default. The custom appearance has some improvements over the web appearance,
    for example you can specify background and border colors.
  * "Web" - Standard form field appearance using native platform styling.
    See https://developer.mozilla.org/en-US/docs/Web/CSS/appearance for details.
  * "Predefined" - Predefined appearance stream from PDF when available. If the appearance stream is not available, custom appearance will be used.
  ```javascript  
  // Example 1: Use platform-native styling for radio and checkbox buttons.
  var viewer = new GcPdfViewer("#root", { fieldsAppearance: { radioButton: "Web", checkBoxButton: "Web" } });
  ```
  ```javascript  
  // Example 2: Use predefined appearance stream for radio buttons:
  var viewer = new GcPdfViewer("#root", { fieldsAppearance: { radioButton: "Predefined" } });
  ```
- Added enableXfa option: render XFA (XML Forms Architecture) forms if any; the default is true. (DOC-3681)
  ```javascript
  // Example: turn XFA forms off:
  var viewer = new GcPdfViewer(selector, { enableXfa: false });
  ```
- Added requireTheme option. Use this option to apply a built-in CSS theme, this will inject the theme styles directly into the page head.
  Note that only a known built-in theme can be specified, otherwise the viewer will fail to load.
  Available built-in themes: "viewer", "dark", "dark-yellow", "gc-blue", "light", "light-blue".
  This option takes precedence over the "theme" option which can be used to specify a custom theme.
  ```javascript
  // Example:
  var viewer = new GcPdfViewer(selector, { 
	   requireTheme: "light"
  });
  ```
- Added onThemeChanged event: raised when the user changes the viewer theme.
  ```javascript
  // Example:
  var viewer = new GcPdfViewer(selector, { 
	   requireTheme: localStorage.getItem('demo_theme') || 'viewer'
   });
   viewer.addDefaultPanels();
   viewer.onThemeChanged.register(function(args) {
     localStorage.setItem('demo_theme', args.theme);
   });
   ```
- Added onInitialized option: the onInitialized handler will be called immediately after the viewer is instantiated.
  ```javascript
  // Example:
  var viewer = new GcPdfViewer("#root", { 
    onInitialized: (viewer)=> { 
      // put viewer initialization code here. 
    } 
  });
  ```
### Fixed
- Multiple bug fixes.

### See __CHANGELOG.&#8203;md__ for detailed release notes.

## See it in action

- [GrapeCity Documents PDF Viewer demo site](https://www.grapecity.com/documents-api-pdfviewer/demos/)
  shows the various features of GcPdfViewer, including features that rely on [_Support API_](#support_api).
  On that site you can also modify the client side code and see the effect of the changes.
- All demos in [GrapeCity Documents for PDF Sample Browser](https://www.grapecity.com/documents-api-pdf/demos/) use GcPdfViewer to show sample PDFs.

## Installation

### To install the latest release version:

```sh
npm install @grapecity/gcpdfviewer
```

### To install from the zip archive:

Go to https://www.grapecity.com/download/documents-pdf and follow the directions on that page to get your 30-day evaluation and deployment license key.
The license key will allow you to develop and deploy your application to a test server.
Unzip the viewer distribution files (list below) to an appropriate location accessible from the web page where the viewer will live.

Viewer zip includes the following files:

- README.&#8203;md (this file)
- CHANGELOG.&#8203;md
- gcpdfviewer.js
- gcpdfviewer.worker.js
- package.json
- index.html (sample page)
- Theme files:
  - themes/dark-yellow.css
  - themes/dark-yellow.jscss
  - themes/light-blue.css
  - themes/light-blue.jscss
  - themes/viewer.css
  - themes/viewer.jscss
- Predefined CMap files for Chinese, Japanese, or Korean text output:
  - resource/bcmaps/*.bin
- TypeScript declaration files:
  - typings/*.*

## Documentation

Online documentation is available [here](https://www.grapecity.com/documents-api-pdf/docs/online/grapecitydocumentspdfviewer.html).

## Licensing

GrapeCity Documents PDF Viewer is a commercially licensed product. Please [visit this page](https://www.grapecity.com/licensing/documents-api) for details.

## Getting more help

GrapeCity Documents PDF Viewer is distributed as part of GrapeCity Documents for PDF.
You can ask any questions about the viewer, or report bugs using the
[Documents for PDF public forum](https://www.grapecity.com/forums/documents-pdf).

## More details on using the viewer

### Adding the viewer to an HTML page:

```HTML
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <!-- Limit content scaling to ensure that the viewer zooms correctly on mobile devices: -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="theme-color" content="#000000" />
    <title>GrapeCity Documents PDF Viewer</title>
    <script type="text/javascript" src="lib/gcpdfviewer.js"></script>
    <script>
        function loadPdfViewer(selector) {
            var viewer = new GcPdfViewer(selector,
              {
                /* Specify options here */
                renderInteractiveForms: true
              });
            // Skip the 'report list' panel:
            // viewer.addReportListPanel();
            viewer.addDefaultPanels();
            // Optionally, open a PDF (will only work if this runs from a server):
            viewer.open('HelloWorld.pdf');
            // Change default viewer toolbar:
            viewer.toolbarLayout.viewer.default = ['$navigation', '$split', 'text-selection', 'pan', '$zoom', '$fullscreen',
              'save', 'print', 'rotate', 'view-mode', 'doc-title'];
            viewer.applyToolbarLayout();
        }
    </script>
  </head>
  <body onload="loadPdfViewer('#root')">
    <div id="root"></div>
  </body>
</html>
```

### How to license the viewer:

Set the GcPdfViewer Deployment key to the GcPdfViewer.License property before you create and initialize GcPdfViewer.
This must precede the code that references the js files.

```javascript
  // Add your license
  GcPdfViewer.LicenseKey = 'xxx';
  // Add your code
  const viewer = new GcPdfViewer("#viewer1", { file: 'helloworld.pdf' });
  viewer.addDefaultPanels();
```

### <a id="support_api"></a>Support API

_Support API_ is a server-side library available as NuGet package
[GrapeCity.Documents.Pdf.ViewerSupportApi](https://www.nuget.org/packages/GrapeCity.Documents.Pdf.ViewerSupportApi/).
The full source code of this library together with C# demo projects for ASP.&#8203;NET Core and Web Forms
is included in the __src/__ folder inside the package (the **WEB_FORMS** constant is defined for the Web Forms target).
In your server solution you can either reference the package, or include the source project and reference it instead.

When GcPdfViewer is connected to a Support API server, its editing features are enabled.
The edits done on the client are accumulated, and when the user clicks 'save',
the original PDF and the edits are sent to the server, the edits are applied (using GcPdf),
and the modified PDF is sent back to the client.

To set up a Support API server on your own system and see it in action,
download any of the samples from the [GcPdfViewer demo site](https://www.grapecity.com/documents-api-pdfviewer/demos/)
(e.g. [Edit PDF](https://www.grapecity.com/documents-api-pdfviewer/demos/edit-pdf/purejs)),
and follow the instructions in the readme.&#8203;md included in the downloaded zip.

NOTE that you will need a GrapeCity Documents for PDF Professional license to use Support API in your apps.

### Keyboard shortcuts

#### Viewer mode

- ```Ctrl +``` - zoom in
- ```Ctrl -``` - zoom out
- ```Ctrl 0``` - zoom to 100%
- ```Ctrl 9``` - zoom to window
- ```Ctrl A``` - select all
- ```R``` - rotate clockwise
- ```Shift R``` - rotate counterclockwise
- ```H``` - enable pan tool
- ```S``` - enable selection tool
- ```V``` - enable selection tool
- ```Ctrl O``` - open local PDF
- ```Ctrl F``` - text search
- ```Ctrl P``` - print
- ```Home``` - go to start of line
- ```Ctrl Home``` - go to start of document
- ```Shift Home``` - select  to start of line
- ```Shift Ctrl Home``` - select  to start of document
- ```End``` - go to end of line
- ```Ctrl End``` - go to end of document
- ```Shift End``` - select  to end of line
- ```Shift Ctrl End``` - select to end of document
- ```Left``` - go left
- ```Shift Left``` - select left
- ```Alt Left``` - go back in history
- ```Right``` - go right
- ```Shift Right``` - select right
- ```Alt Right``` - go forward in history
- ```Up``` - go up
- ```Shift Up``` - select up
- ```Down``` - go down
- ```Shift Down``` - select down
- ```PgUp``` - page up
- ```PgDown``` - page down
- ```Shift PgUp``` - select page up
- ```Shift PgDown``` - select page down

#### Editing modes

- ```Delete``` - delete selected annotation/field
- ```Esc``` - unselect annotation/field
- ```Ctrl Z``` - undo
- ```Ctrl Y``` - redo
- ```Ctrl Shift Z``` - redo

### Toolbar items

The default viewer toolbar items layout (items starting with '$' are inherited from the base viewer, other items are PDF viewer specific.):

```
['open', '$navigation', '$split', 'text-selection', 'pan', '$zoom', '$fullscreen', 'rotate', 'view-mode', 'theme-change', 'download', 'print', 'save', 'hide-annotations', 'doc-title', 'doc-properties', 'about']
```

The full list of the PDF-viewer specific toolbar items:

```
'text-selection', 'pan', 'open', 'save', 'download', 'print', 'rotate', 'theme-change', 'doc-title', 'view-mode', 'single-page', 'continuous-view'
```

The full list of base viewer toolbar items:

```
'$navigation' '$refresh', '$history', '$mousemode', '$zoom', '$fullscreen', '$split'
```

You can get default base viewer items by using the getDefaultToolbarItems() method, e.g.:

```javascript
const toolbar: Toolbar = viewer.toolbar;
let buttons = toolbar.getDefaultToolbarItems();
buttons = buttons.filter(b => b !== '$refresh');
```

To specify a custom set of toolbar items, use the toolbarLayout property and applyToolbarLayout() method, e.g.:

```javascript
viewer.toolbarLayout.viewer = {
  default: ["$navigation", 'open', '$split', 'doc-title'],
  fullscreen: ['$fullscreen', '$navigation'],
  mobile: ["$navigation", 'doc-title']
};
viewer.toolbarLayout.annotationEditor = {
  default: ['edit-select', 'save', '$split', 'edit-text'],
  fullscreen: ['$fullscreen', 'edit-select', 'save', '$split', 'edit-text'],
  mobile: ['$navigation']
};
viewer.toolbarLayout.formEditor = {
  default: ['edit-select-field', 'save', '$split', 'edit-widget-tx-field', 'edit-widget-tx-password'],
  fullscreen: ['$fullscreen', 'edit-select-field', 'save', '$split', 'edit-widget-tx-field', 'edit-widget-tx-password'],
  mobile: ['$navigation']
};
viewer.applyToolbarLayout();
```

Here is an example of how to create your own custom toolbar button:

```javascript
const toolbar: Toolbar = viewer.toolbar;
toolbar.addItem({
  key: 'my-custom-button',
  iconCssClass: 'mdi mdi-bike',
  title: 'Custom button',
  enabled: false,
  action: () => {
    alert("Custom toolbar button clicked");
  },
  onUpdate: (args) => ({ enabled: viewer.hasDocument }),
});
viewer.toolbarLayout.viewer.default =  ['$navigation', 'my-custom-button'];
viewer.applyToolbarLayout();
```

### Using the viewer in Preact

Add a reference to the viewer script.

```HTML
<body>
  <script type="text/javascript" src="/lib/gcpdfviewer/gcpdfviewer.js"></script>
  ...
```

Add the placeholder to your App template, e.g.:

```HTML
<section id="pdf"></section>
```

Render the viewer:

```javascript
let viewer = new GcPdfViewer('section#pdf');
viewer.addDefaultPanels();
```

---

# <a id="japanese"></a>GrapeCity PDF ビューワ（日本語版）

__GrapeCity PDF ビューワ__ (__GcPdfViewer__) は、主要なブラウザで動作する、JavaScriptベースのPDFビューワおよびエディタです。Windows、MAC、Linux、iOS、Androidなどのデバイス上でPDFドキュメントを表示 (または編集 - 下記の _サポートAPI_ を参照) するためのクロスプラットフォームソリューションとして使用することができます。
なお、本PDFビューワは、クロスプラットフォーム環境にてPDFを作成・編集できるAPIライブラリ [DioDocs for PDF](https://www.grapecity.co.jp/developer/diodocs/pdf) に付属する製品となります。

[_サポートAPI_](#support_api_ja) は、サーバーサイドのNuGetパッケージ
「[GrapeCity.DioDocs.Pdf.ViewerSupportApi.ja](https://www.nuget.org/packages/GrapeCity.DioDocs.Pdf.ViewerSupportApi.ja/)」で、
GcPdfに接続するサーバーをASP.&#8203;NET CoreまたはWeb Formsにて簡単に構築して、
PDFビューワにエディタ機能を追加できるようにするものです。
_サポートAPI_ サーバーに接続すると、PDFビューワをPDFエディタとして使用し、
PDFの新規作成、既存のPDFの編集、PDFフォームの入力や保存、機密コンテンツの削除（墨消し）、
注釈／フォームフィールドの追加／編集などを行うことができるようになります。

PDFビューワのクライアントAPIの詳細については、[こちら](https://docs.grapecity.com/help/diodocs/pdfviewer/)をご参照ください。

製品の特徴：
- IE11、Edge、Chrome、FireFox、Opera、Safariを含むすべての最新ブラウザで動作します
- _サポートAPI_ 経由で _GcPdf_  に接続した場合、以下の機能を提供します：
  - カスタマイズ可能でモバイルフレンドリーなフォームフィラー
  - リアルタイムなコラボレーションモード
  - 注釈とフォームの編集
  - セカンドツールバーを使用したクイック編集
  - PDFコンテンツの墨消し
  - 署名の検証
  - その他の編集機能
- React、Preact、Angular などのフレームワークで動作します
- フォームの入力に対応：記入済みフォームの印刷や、フォームデータのサーバへの送信にも対応しています
- XFA（XML Forms Architecture）フォームに対応しています
- 縦書きテキストや右横書きテキストを含む、テキストの選択/コピーのためのキャレットを提供します
- サムネイル、テキスト検索、ブックマーク、添付ファイル、アーティクル、レイヤー、構造ツリーのパネルが含まれます
- ローカルディスクからPDFファイルを開くことができます
- テキスト、フリーテキスト、リッチテキストなどの注釈に対応しています
- 墨消し注釈（APストリームを含む）に対応し、墨消しの表示・非表示が可能です
- 音声注釈に対応しています
- 文書の回転や、回転させた文書の印刷が可能です
- アーティクルのスレッドナビゲーションに対応しています
- 添付ファイル（添付ファイル注釈と文書レベルの添付ファイルの両方）に完全に対応しています
- 複数のテーマがすでに備わっており、さらに新しいカスタムテーマを追加することも可能です
- 外部のCMapに対応しています
- その他、様々な機能を提供しています

## リリースノート
日本語版として動作を保証しているのは、リリースノートに記載しているバージョンのみとなります。

## [3.1.4] - 2022年5月25日
### 追加
- サイドパネルにサイズ変更のハンドルを追加しました。
- invalidate メソッドを追加しました。ビューワのすべての視覚的要素を適切にレイアウト更新します。
- requiredSupportApiVersion プロパティを追加しました。PDF ビューワの現在のバージョンと互換性のある SupportApi の必要バージョンを取得します。
- supportApiVersion プロパティを追加しました。接続された SupportApi のバージョンがあれば取得します。
- gcPdfVersion プロパティを追加しました。接続された SupportApi で使用されている GcPdf ライブラリのバージョンがあれば取得します。
- 回転ハンドルを使用して、スタンプ注釈を回転できるようになりました。\
  なお、Shift キーを押すと、回転角度が90度の倍数になるよう制御できます。
- XFA フォームの印刷、送信、リセット、JavaScript アクション、ハイパーリンクに対応しました。
- PDF を保存する際、レイヤーの表示状態も保存できるようになりました。
- orientation プロパティを使用して、フォームフィールドのテキストの方向を変更できるようになりました。
- エディタにて、ツールバーボタンの機能を連続実行できるようになりました。\
  toolbarLayout に stickyBehavior プロパティを追加しました。連続実行できるようにするボタンのキーを持つ配列を設定します。
  ただし、連続実行できるのは、注釈／フォームエディタのツールバーボタンだけです。\
  連続実行できるボタンの全リストは次のとおりです：'edit-sign-tool', 'edit-text', 'edit-free-text', 'edit-ink', 'edit-square', 'edit-circle', 'edit-line', 'edit-polyline', 'edit-polygon', 'edit-stamp', 'edit-file-attachment',
  'edit-sound', 'edit-link', 'edit-redact', 'edit-widget-tx-field', 'edit-widget-tx-password',
  'edit-widget-tx-text-area', 'edit-widget-btn-checkbox', 'edit-widget-btn-radio',
  'edit-widget-btn-push', 'edit-widget-ch-combo', 'edit-widget-ch-list-box', 'edit-widget-tx-comb',
  'edit-widget-btn-submit', 'edit-widget-btn-reset', 'edit-erase-field'
```javascript
  // 例：四角形注釈、円注釈、線注釈、墨消し注釈のボタンを連続実行できるようにします。
  viewer.toolbarLayout.stickyBehavior = ["edit-square", "edit-circle", "edit-line", "edit-redact"];
```
- fieldsAppearance オプションを追加しました。ボタンフィールドのレンダリング方法を指定します。
```javascript  
  // 例1：プッシュボタンの外観に［Web］を設定します。
  var viewer = new GcPdfViewer("#root", { fieldsAppearance: { pushButton: "Web" } });
  // 例2：プッシュボタンの外観に［Predefined］を設定します。
  var viewer = new GcPdfViewer("#root", { fieldsAppearance: { pushButton: "Predefined" } });
```
- プログラムにて左サイドバーを非表示にできるようになりました。
```javascript
  // サイドバーを非表示にします。
  viewer.toggleSidebar(false);
  // 以下のコードでも実現できます。
  viewer.leftSidebar.hide();
```
- プログラムにてツールバーを表示／非表示にできるようになりました。
```javascript
  // 例：
  viewer.toggleToolbar(false);
```
- hideAnnotationPopups オプションを追加しました。このオプションを使用すると、すべての注釈ポップアップを非表示にできます。
```javascript
  // 例：
  var viewer = new GcPdfViewer("#root", { hideAnnotationPopups: true });
```
- 現在読み込んでいるドキュメントを閉じることができるようになりました。
```javascript
  // 例：
  await viewer.close();
```
- 次のイベントを追加しました：onBeforeAddAnnotation, onAfterAddAnnotation, onBeforeUpdateAnnotation, onAfterUpdateAnnotation,
  onBeforeRemoveAnnotation, onAfterRemoveAnnotation
```javascript
  // 例：
  viewer.onBeforeAddAnnotation.register(function(args) { console.log(args); });
```
  なお、BeforeAddAnnotation／BeforeUpdateAnnotation／BeforeRemoveAnnotation イベントはキャンセル可能です。
```javascript
  // イベントキャンセルの例：
  viewer.getEvent("BeforeAddAnnotation").register(function(args) { args.cancel = true; });
  viewer.getEvent("BeforeUpdateAnnotation").register(function(args) {
    args.cancel = true;
    viewer.repaint();
  });
```
- 独自のイベントを待ち受けし、トリガーできるようになりました。
```javascript
  // 独自のイベントを待ち受けする例：
  viewer.getEvent("CustomEvent").register(function(args) {
    console.log(args);
  });
  // 独自のイベントをトリガーする例：
  viewer.triggerEvent("CustomEvent", { arg1: 1, arg2: 2});
```
- open() メソッドに認証やその他の HTTP ヘッダーを指定できるようになりました。
```javascript
  // アクセス認証と独自のヘッダーを指定する例：
  viewer.open("http://example.com//pdfs/GetPdf?file=HelloWorld.pdf", { 
    headers: { 
      "Authorization": "Basic " + btoa(unescape(encodeURIComponent("USERNAME:PASSWORD"))),
      "CustomHeader": "Custom header value"
    }
  });
```
 - SupportApi のリクエストに認証やその他の HTTP ヘッダーを指定できるようになりました。
```javascript
  // アクセス認証と独自のヘッダーを指定する例：
  const viewer = new GcPdfViewer("#viewer", {
    supportApi: {
      apiUrl: "192.168.0.1/support-api",
      requestInit: { 
        headers: {
          "Authorization": "Basic " + btoa(unescape(encodeURIComponent("USERNAME:PASSWORD"))),
          "CustomHeader": "Custom header value"
        }
      }
    }
  });
```
- コンボボックスの設定項目に「編集を許可」を追加しました。
- フォームエディタの「タブの順序」の選択項目に「注釈配列」と「ウィジェット」を追加しました。

### 変更
- 接続している SupportApi のバージョンが 0.0.0.0（ソースからのビルド）の場合、バージョン不一致の警告を表示しないよう変更しました。

##### 詳細なリリースノートについては、 __CHANGELOG-JP.&#8203;md__ をご参照ください。

## 製品デモ
- [PDFビューワの製品デモ](https://demo.grapecity.com/diodocs/pdfviewer/demos/) では、[_サポートAPI_](#support_api_ja) を使用する編集機能を含め、
  PDFビューワの様々な機能を紹介しています。
  このデモでは、クライアント側のコードを変更し、どのように反映されるか確認することもできます。
- [DioDocs for PDFの製品デモ](https://demo.grapecity.com/diodocs/pdf/) 内のすべてのデモでは、PDFビューワを使用してPDFを表示しています。

## インストール
```sh
npm install @grapecity/gcpdfviewer
```

## 製品ヘルプ
製品ヘルプは[こちら](https://docs.grapecity.com/help/diodocs/pdf/#grapecitydocumentspdfviewer.html)からご覧いただけます。

## ライセンス
GrapeCity PDF ビューワのご利用にはライセンスが必要となります。詳しくはWebサイトの[ライセンス手続き](https://www.grapecity.co.jp/developer/support/license#docapi)ページをご覧ください。

## 製品情報
GrapeCity PDF ビューワは、DioDocs for PDF の一部として提供しております。製品に関する詳細な情報については、Webサイトの[製品情報](https://www.grapecity.co.jp/developer/diodocs/pdf)ページをご参照ください。

## PDFビューワの詳細な使用方法
### HTMLページへのPDFビューワの追加
```HTML
<!DOCTYPE html>
<html lang="ja">
<head>
    <meta charset="utf-8">
    <!--コンテンツの拡大縮小を制限して、モバイルデバイスでビューワが正しくズームできるようにします。-->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="theme-color" content="#000000" />
    <title>GrapeCity PDF ビューワ</title>
    <script type="text/javascript" src="lib/node_modules/@grapecity/gcpdfviewer/gcpdfviewer.js"></script>
    <script>
        function loadPdfViewer(selector) {
            var viewer = new GcPdfViewer(selector,
                {
                    /* ここでオプションを指定します */
                    renderInteractiveForms: true
                });
            viewer.addDefaultPanels();
            // 必要に応じてPDFを開きます（サーバーから実行時にのみ機能）
            viewer.open('HelloWorld.pdf');
            // デフォルトのビューワのツールバーを変更します
            viewer.toolbarLayout.viewer.default = ['$navigation', '$split', 'text-selection', 'pan', '$zoom', '$fullscreen',
                'save', 'print', 'rotate', 'view-mode', 'doc-title'];
            viewer.applyToolbarLayout();
        }
    </script>
</head>
<body onload="loadPdfViewer('#root')">
    <div id="root"></div>
</body>
</html>
```

### PDFビューワへのライセンスの適用
GcPdfViewer のインスタンスを作成して初期化する前に、PDFビューワのライセンスキーを GcPdfViewer.License プロパティに設定します。
なお、これは jsファイルを参照するコードの前に記述する必要があります。

```javascript
  // ご自身のライセンスを追加してください
  GcPdfViewer.LicenseKey = 'xxx';
  // 適宜コードを追加してください
  const viewer = new GcPdfViewer('#root', { file: 'helloworld.pdf' });
  viewer.addDefaultPanels();
```

### PDFビューワの検索オプション
PDFビューワには、以下のようなテキストの検索オプションが用意されていますが、**英語のみの対応**となっており、日本語は正しく検索できない場合があります。
- 大/小文字を区別
- 単語単位で検索
- 単語の先頭を検索
- 単語の末尾を検索
- ワイルドカード
- 近接

上記検索オプションを非表示にしたい場合は、ページの`<head>`部分に以下のとおりCSSスタイルを追加してください。
```HTML
<style>
    .gc-viewer .search .search__query-params > label:nth-child(1),
    .gc-viewer .search .search__query-params > label:nth-child(2),
    .gc-viewer .search .search__query-params > label:nth-child(3),
    .gc-viewer .search .search__query-params > label:nth-child(4),
    .gc-viewer .search .search__query-params > label:nth-child(5),
    .gc-viewer .search .search__query-params > label:nth-child(6) {
        display: none;
    }
</style>
```

### <a id="support_api_ja"></a>サポートAPI
_サポートAPI_ は、NuGetパッケージ
「[GrapeCity.DioDocs.Pdf.ViewerSupportApi.ja](https://www.nuget.org/packages/GrapeCity.DioDocs.Pdf.ViewerSupportApi.ja/)」として
提供されるサーバーサイドのライブラリです。
このライブラリの完全なソースコードは、ASP.&#8203;NET CoreおよびWeb FormsのC＃デモプロジェクトとともに、
パッケージ内の「 __src/__ 」フォルダに含まれています（Web Formsのために **WEB_FORMS** 定数が定義されています）。
サーバーの構築方法として、パッケージを参照するか、または代わりにソースプロジェクトを含めてそれを参照するか、選ぶことができます。

PDFビューワがサポートAPIサーバーに接続されている場合、編集機能が有効になります。
クライアント上で行われた編集は蓄積され、ユーザーが「保存」をクリックすると、
元のPDFと編集内容がサーバーに送られて、編集が（GcPdfを使って）適用され、
変更されたPDFがクライアントに送り返されます。

サポートAPIサーバーをセットアップして動作を確認するには、
[PDFビューワの製品デモ](https://demo.grapecity.com/diodocs/pdfviewer/demos/)から任意のサンプルをダウンロードし
（例：[PDFの編集](https://demo.grapecity.com/diodocs/pdfviewer/demos/edit-pdf/purejs)）、
ダウンロードしたzipに含まれるreadme.&#8203;mdの説明をご参照ください。

なお、サポートAPIを使用するには、有効なDioDocs for PDFのライセンスが必要です。
ライセンスは、GcPdfDocumentのSetLicenseKey静的メソッドにて設定します。以下は、ASP.&#8203;NET Coreプロジェクトの「Startup.cs」にてDioDocs for PDFライセンスをサポートAPIに適用する例です。
```C#
public class Startup
{
    static Startup()
    {
        // 略
        GcPdfDocument.SetLicenseKey("ライセンスキー");
    }
    // 略
}
```

### キーボードのショートカット
#### 表示モード
- ```Ctrl +``` - 拡大
- ```Ctrl -``` - 縮小
- ```Ctrl 0``` - 100％に拡大
- ```Ctrl 9``` - ページ幅に合わせて拡大
- ```Ctrl A``` - すべて選択
- ```R``` - ドキュメントを右回りに回転
- ```Shift R``` - ドキュメントを左回りに回転
- ```H``` - 手のひらツールを有効化
- ```S``` - 選択ツールを有効化
- ```Ctrl O``` - PDFファイルを開く
- ```Ctrl F``` - テキスト検索
- ```Ctrl P``` - 印刷
- ```Home``` - 行頭に移動
- ```Ctrl Home``` - ドキュメントの先頭に移動
- ```Shift Home``` - 行頭まで選択
- ```Shift Ctrl Home``` - ドキュメントの先頭まで選択
- ```End``` - 行末に移動
- ```Ctrl End``` - ドキュメントの末尾に移動
- ```Shift End``` - 行末まで選択
- ```Shift Ctrl End``` - ドキュメントの末尾まで選択
- ```Left``` - 左に移動
- ```Shift Left``` - 左を選択
- ```Alt Left``` - 前の履歴に戻る
- ```Right``` - 右に移動
- ```Shift Right``` - 右を選択
- ```Alt Right``` - 次の履歴に進む
- ```Up``` - 上に移動
- ```Shift Up``` - 上まで選択
- ```Down``` - 下に移動
- ```Shift Down``` - 下まで選択
- ```PgUp``` - 前のページに移動
- ```PgDown``` - 次のページに移動
- ```Shift PgUp``` - 前のページまで選択
- ```Shift PgDown``` - 次のページまで選択

#### 編集モード
- ```Delete``` - 選択した注釈/フィールドを削除
- ```Esc``` - 注釈/フィールドの選択を解除
- ```Ctrl Z``` - 元に戻す
- ```Ctrl Y``` - やり直す
- ```Ctrl Shift Z``` - やり直す

### ツールバーの項目
PDFビューワのデフォルトのツールバーレイアウトは以下のとおりです。（'$'で始まる項目は、ビューワの標準として備わっているものであり、それ以外の項目はPDFビューワ固有のものになります。）
```
['open', '$navigation', '$split', 'text-selection', 'pan', '$zoom', '$fullscreen', 'rotate', 'view-mode', 'theme-change', 'download', 'print', 'save', 'hide-annotations', 'doc-title', 'doc-properties', 'about']
```

PDFビューワ固有のツールバー項目は以下のとおりです。
```
'text-selection', 'pan', 'open', 'save', 'download', 'print', 'rotate', 'theme-change', 'doc-title', 'view-mode', 'single-page', 'continuous-view'
```

ビューワの標準のツールバー項目は以下のとおりです。
```
'$navigation' '$refresh', '$history', '$mousemode', '$zoom', '$fullscreen', '$split'
```

以下のように、getDefaultToolbarItems() メソッドを使用することで、デフォルトのツールバー項目を取得することができます。
```javascript
const toolbar: Toolbar = viewer.toolbar;
let buttons = toolbar.getDefaultToolbarItems();
buttons = buttons.filter(b => b !== '$refresh');
```

ツールバー項目をカスタマイズするには、以下のように toolbarLayout プロパティと applyToolbarLayout() メソッドを使用します。
```javascript
viewer.toolbarLayout.viewer = {
  default: ["$navigation", 'open', '$split', 'doc-title'],
  fullscreen: ['$fullscreen', '$navigation'],
  mobile: ["$navigation", 'doc-title']
};
viewer.toolbarLayout.annotationEditor = {
  default: ['edit-select', 'save', '$split', 'edit-text'],
  fullscreen: ['$fullscreen', 'edit-select', 'save', '$split', 'edit-text'],
  mobile: ['$navigation']
};
viewer.toolbarLayout.formEditor = {
  default: ['edit-select-field', 'save', '$split', 'edit-widget-tx-field', 'edit-widget-tx-password'],
  fullscreen: ['$fullscreen', 'edit-select-field', 'save', '$split', 'edit-widget-tx-field', 'edit-widget-tx-password'],
  mobile: ['$navigation']
};
viewer.applyToolbarLayout();
```

以下のように、独自のカスタムツールバーボタンを作成することもできます。
```javascript
const toolbar: Toolbar = viewer.toolbar;
toolbar.addItem({
  key: 'my-custom-button',
  iconCssClass: 'mdi mdi-bike',
  title: 'カスタムボタン',
  enabled: false,
  action: () => {
    alert("カスタムツールバーボタンがクリックされました");
  },
  onUpdate: (args) => ({ enabled: viewer.hasDocument }),
});
viewer.toolbarLayout.viewer.default =  ['$navigation', 'my-custom-button'];
viewer.applyToolbarLayout();
```

### Preact でのPDFビューワの使用
PDFビューワのスクリプトへの参照を追加します。
```HTML
<body>
  <script type="text/javascript" src="/lib/gcpdfviewer/gcpdfviewer.js"></script>
  ...
```

アプリのテンプレートに以下のようにプレースホルダを追加します。
```HTML
<section id="pdf"></section>
```

PDFビューワをレンダリングします。
```javascript
let viewer = new GcPdfViewer('section#pdf');
viewer.addDefaultPanels();
```

---
_The End._
