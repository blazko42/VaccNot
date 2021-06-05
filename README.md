# vaccnot

Automatic notifications for available booster slots regarding covid 19 vaccine.

It uses the romanian vaccination platform APIs to check for available vaccination slots. App requires a valid account created on the vaccination platform in order for it to work.

See settings.cs for relevant information you need to gather from the vaccination platform. (use network tab in browser console).

See notification.cs to set up your desired notification email.

Vaccination platform requires a session id sent in the header of the request everytime you call an api therefore a workaround was created using selenium for edge. A page to the login of the platform is opened in the background, it logs you in and fetches the session id which is stored and later used in the api request for booster slots fetch. 

App contains a simple interface in which : you can see the settings you set in settings.cs, proceed to actively watch for booster slots in real time (checks every 30 minutes and send you a customised email if there are slots found) and/or check for booster slots once on request.

NOTE: This app is now obsolete since the april 2021 update which rendered the functions described above useless. Posted here for personal learning purposes. 
