import { useOidcUser } from '@axa-fr/react-oidc';
import styled from '@emotion/styled';
import { Spin } from 'antd';
import { FC, useMemo } from 'react';

import { PartnerCreateModel, PartnerReadModel, PartnerType } from '~/types';

import { ApplicationForm } from './ApplicationForm';
import { getStored } from './getStored';

type PrepareFormProps = { data?: PartnerReadModel };

const SpinContainer = styled.div`
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const LoadingComponent = () => {
  return (
    <SpinContainer>
      <Spin size="large" />
    </SpinContainer>
  );
};

const PrepareForm: FC<PrepareFormProps> = ({ data }) => {
  const { oidcUser } = useOidcUser();

  const defaultValues = useMemo<PartnerCreateModel>(() => {
    const stored = data ?? getStored();

    return {
      phoneComfinmationCode: '',
      registrationAddress: {
        house: '',
        street: '',
      },
      contactEmail: stored.contactEmail ?? oidcUser?.email ?? '',
      contactPhone: stored.contactPhone ?? '',
      inn: stored.inn ?? '',
      title: stored.title ?? '',
      type: stored.type ?? PartnerType.SelfEmployed,
      acceptedTermsOfService: true,
    };
  }, [data, oidcUser?.email]);

  return oidcUser ? <ApplicationForm completed={!!data?.state} defaultValues={defaultValues} /> : <LoadingComponent />;
};

export { PrepareForm };
