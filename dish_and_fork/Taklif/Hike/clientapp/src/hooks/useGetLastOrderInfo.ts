import { useCallback } from 'react';

import { useConfig } from '~/contexts';
import { LastOrderInfo } from '~/types';

type UseGetLastOrderInfo = () => () => LastOrderInfo;

const useGetLastOrderInfo: UseGetLastOrderInfo = () => {
  const { storageKeys } = useConfig();

  return useCallback(() => {
    try {
      const saved = window.localStorage.getItem(storageKeys.lastOrderDataStorageKey);
      const parsed = saved ? JSON.parse(saved) : {};

      return parsed && typeof parsed === 'object' ? parsed : {};
    } catch (e) {
      return {};
    }
  }, [storageKeys.lastOrderDataStorageKey]);
};

export { useGetLastOrderInfo };
export type { LastOrderInfo, UseGetLastOrderInfo };
