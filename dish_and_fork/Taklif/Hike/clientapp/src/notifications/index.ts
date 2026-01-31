import { initializeApp } from 'firebase/app';
import { getMessaging, getToken } from 'firebase/messaging';

import config from '~/config';

export const initFirebase = async () => {
  const { firebaseConfig } = config.messaging;

  // Initialize Firebase
  const app = initializeApp(firebaseConfig);

  if ('Notification' in window && 'serviceWorker' in navigator) {
    console.log('Notifications enabled.');

    const messaging = getMessaging(app);
    const swName = `/firebase-messaging-sw.js?firebaseConfig=${btoa(JSON.stringify(firebaseConfig))}`;

    let swRegistration = await navigator.serviceWorker.getRegistration(swName);

    if (!swRegistration) {
      swRegistration = await navigator.serviceWorker.register(swName, {
        type: 'module',
      });
    }

    const permission = await Notification.requestPermission();

    if (permission === 'granted') {
      const token = await getToken(messaging, {
        serviceWorkerRegistration: swRegistration,
        vapidKey: config.messaging.vapidKey,
      });

      window.localStorage.setItem(config.messaging.localStorageToken, token);
    }

    return messaging;
  }

  return null;
};
