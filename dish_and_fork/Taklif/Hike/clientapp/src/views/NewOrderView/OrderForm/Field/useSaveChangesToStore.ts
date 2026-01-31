import { useCallback } from 'react';

import { useConfig } from '~/contexts';
import { useGetLastOrderInfo } from '~/hooks';

import { InputProps } from './Input';

type UseSaveChangesToStore = (name: string) => Required<InputProps>['onChange'];

const useSaveChangesToStore: UseSaveChangesToStore = (name) => {
  const { storageKeys } = useConfig();
  const getLastOrderInfo = useGetLastOrderInfo();

  return useCallback(
    (event) => {
      const { value } = event.currentTarget;

      try {
        const saved = getLastOrderInfo();
        window.localStorage.setItem(storageKeys.lastOrderDataStorageKey, JSON.stringify({ ...saved, [name]: value }));
      } catch (e) {
        window.localStorage.setItem(storageKeys.lastOrderDataStorageKey, JSON.stringify({ [name]: value }));
      }
    },
    [getLastOrderInfo, name, storageKeys.lastOrderDataStorageKey],
  );
};

export { useSaveChangesToStore };
export type { UseSaveChangesToStore };
