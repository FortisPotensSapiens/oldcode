import { useOidc } from '@axa-fr/react-oidc';
import { useState } from 'react';

import { useGetConfig, useGetMyUserProfile } from '~/api';

import { AgreementConfirmModal } from './AgreementConfirmModal/AgreementConfirmModal';
import { IS_STORE_AGREEMENT_STORAGE_KEY } from './types';

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
    (isAuthenticated && localStorage.getItem(IS_STORE_AGREEMENT_STORAGE_KEY))
  ) {
    return <>{children}</>;
  }

  const hideModal = () => {
    setOpen(false);
  };

  return (
    <>
      <AgreementConfirmModal open={open} hideModal={hideModal} url={String(configData.partnerTermOfServiceFilePath)} />
      {children}
    </>
  );
};

export { AgreementWrapper };
