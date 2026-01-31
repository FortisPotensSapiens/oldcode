import { Alert } from 'antd';
import { FC, useCallback } from 'react';

import { useGetPartnerMy } from '~/api';
import { Error, LoadingSpinner } from '~/components';
import { PartnerState } from '~/types';

import { PartnerExternalIdView } from './PartnerExternalIdView';
import { PartnerForm } from './PartnerForm';

const PartnerSettingsView: FC = () => {
  const { data, isError, isLoading, refetch } = useGetPartnerMy();

  const onUpdated = useCallback(() => {
    refetch();
  }, [refetch]);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError || !data) {
    return <Error />;
  }

  return (
    <>
      {data.state !== PartnerState.Confirmed ? <Alert message="На модерации" /> : undefined}

      {data ? <PartnerExternalIdView externalId={data.externalId ?? ''} /> : undefined}
      <PartnerForm data={data} onUpdated={onUpdated} />
    </>
  );
};

export { PartnerSettingsView };
