import { useCallback } from 'react';

import { usePostPartner } from '~/api';
import { PartnerCreateModel } from '~/types';

type UseSubmit = (ready?: boolean) => [
  (data: PartnerCreateModel) => void,
  {
    data?: string;
    isError: boolean;
    isLoading: boolean;
    isSuccess: boolean;
  },
];

const useSubmit: UseSubmit = (ready) => {
  const { data, isError, isLoading, isSuccess, mutate } = usePostPartner();

  const handler = useCallback(
    (data: PartnerCreateModel) => {
      if (ready) {
        mutate(data);
      }
    },
    [mutate, ready],
  );

  return [handler, { data, isError, isLoading, isSuccess }];
};

export { useSubmit };
