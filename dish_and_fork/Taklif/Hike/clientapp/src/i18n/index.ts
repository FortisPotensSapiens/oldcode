import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import ru from '../translations/ru/common.json';

i18n.use(initReactI18next).init({
  resources: {
    ru: {
      translation: ru,
    },
  },
  lng: 'ru',
  interpolation: {
    escapeValue: false,
  },
});

export default i18n;
