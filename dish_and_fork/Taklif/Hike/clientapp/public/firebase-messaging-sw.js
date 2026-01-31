// eslint-disable-next-line import/no-unresolved
import { initializeApp } from 'https://www.gstatic.com/firebasejs/9.17.1/firebase-app.js';
// eslint-disable-next-line import/no-unresolved
import { getMessaging } from 'https://www.gstatic.com/firebasejs/9.17.1/firebase-messaging-sw.js';

// eslint-disable-next-line no-restricted-globals
const swScriptUrl = new URL(self.location);

const base64Config = swScriptUrl.searchParams.get('firebaseConfig');
const decoded = atob(base64Config);

// Initialize Firebase
const app = initializeApp(JSON.parse(decoded));

export const messaging = getMessaging(app);
