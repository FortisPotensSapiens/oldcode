import { useOidc } from '@axa-fr/react-oidc';
import { useState } from 'react';

import { useGetConfig, useGetMyUserProfile } from '~/api';

import { AgreementConfirmModal } from './AgreementConfirmModal/AgreementConfirmModal';

const AgreementWrapper: React.FC = ({ children }) => {
  const { data: configData } = useGetConfig();
  const { isAuthenticated } = useOidc();
  const { data } = useGetMyUserProfile({
    enabled: isAuthenticated,
  });
  const [open, setOpen] = useState(true);

  if (
    !data ||
    !configData ||
    !isAuthenticated ||
    (isAuthenticated &&
      data?.acceptedPivacyPolicy &&
      data?.acceptedConsentToPersonalDataProcessing &&
      data?.acceptedTermsOfUse &&
      data?.acceptedOfferFoUser)
  ) {
    return <>{children}</>;
  }

  const hideModal = () => {
    setOpen(false);
  };

  return (
    <>
      <AgreementConfirmModal
        open={open}
        hideModal={hideModal}
        docs={[
          {
            title: 'согласием на обработку персональных данных',
            required: true,
            name: 'acceptedConsentToPersonalDataProcessing',
            url: String(configData.consentToPersonalDataProcessingFilePath),
          },
          {
            title: 'политикой конфеденциальности',
            required: true,
            name: 'acceptedPivacyPolicy',
            url: String(configData.privacyPolicyFilePath),
          },
          {
            title: 'условием использования',
            required: true,
            name: 'acceptedTermsOfUse',
            url: String(configData.termsOfUserFilePath),
          },
          {
            title: 'офертой для физических лиц на заключение договоров купли-продажи',
            required: true,
            name: 'acceptedOfferFoUser',
            url: String(configData.offerForBuyerFilePath),
          },
          {
            title: 'согласием на рассылку мне новостных, маркентинговых и иных данных',
            required: false,
            name: 'acceptedConsentToMailings',
            url: String(configData.consentToMailingsFilePath),
          },
        ]}
      />
      {children}
    </>
  );
};

export { AgreementWrapper };
