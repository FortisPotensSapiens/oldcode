import { ConfigModel } from '~/types';

export const CONFIG_DOCUMENTS_MAPPING: (keyof ConfigModel)[] = [
  'termsOfUserFilePath',
  'consentToPersonalDataProcessingFilePath',
  'consentToMailingsFilePath',
  'privacyPolicyFilePath',
  'offerForBuyerFilePath',
  'partnerTermOfServiceFilePath',
];

export const CONFIG_DOCUMENTS_TRANSLATIONS: Record<string, string> = {
  consentToMailingsFilePath: 'Согласие на рассылки',
  consentToPersonalDataProcessingFilePath: 'Согласие на обработку персональных данных',
  privacyPolicyFilePath: 'Политика конфиденциальности',
  termsOfUserFilePath: 'Условия использований',
  partnerTermOfServiceFilePath: `Публичная оферта для самозанятых, индивидуальных
предпринимателей и юридических лиц на заключение агентского
договора с помощью Сервиса`,
  offerForBuyerFilePath:
    'Публичная оферта для физических лиц на заключение договора купли-продажи Изделия с помощью Сервиса',
};
