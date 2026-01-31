import { useCallback } from 'react';
import { useQueryClient } from 'react-query';

import { usePostAdminConfirmPartner } from '~/api';
import { useConfig } from '~/contexts';
import { PartnerReadModel, PartnerState } from '~/types';

type SuccessHandler = (payload: PartnerReadModel) => void;

type UseClick = (row: PartnerReadModel) => [
  () => void,
  {
    isError: boolean;
    isLoading: boolean;
    isSuccess: boolean;
  },
];

const useClick: UseClick = (row) => {
  const { api } = useConfig();
  const queryClient = useQueryClient();

  const { isError, isLoading, isSuccess, mutate } = usePostAdminConfirmPartner({
    onSuccess: (data, id) => {
      const changes = queryClient.getQueryData<PartnerReadModel[]>([api.keys.getPartners]);
      const changed = changes?.find((item) => item.id === id);

      if (changed) {
        changed.state = PartnerState.Confirmed;
      }

      queryClient.setQueryData([api.keys.getPartners], changes);
    },
  });

  const handler = useCallback(async () => {
    try {
      mutate(row.id);
    } catch (e) {
      console.error(`Can't confirm partner ${row.id}`);
    }
  }, [mutate, row.id]);

  return [handler, { isError, isLoading, isSuccess }];
};

export { useClick };
export type { SuccessHandler, UseClick };
