import { FC } from 'react';
import { Navigate } from 'react-router-dom';

import { useGetMyPartnership } from '~/api';
import { LoadingSpinner } from '~/components';
import { getPartnerSettingsPath } from '~/routing';
import { PartnerState } from '~/types';

import { AgreementWrapper } from './AgreementWrapper/AgreementWrapper';
import { PrepareForm } from './PrepareForm';

const BecomeSeller: FC = () => {
  const { data, isLoading } = useGetMyPartnership(true, true);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (data?.state === PartnerState.Confirmed) {
    return <Navigate replace to={getPartnerSettingsPath()} />;
  }

  return (
    <AgreementWrapper>
      <PrepareForm data={data} />
    </AgreementWrapper>
  );
};

export { BecomeSeller };
