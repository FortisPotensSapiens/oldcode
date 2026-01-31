import { useMemo } from 'react';

import { MenuItem } from '../types';
import { useAdminMenu } from '../useAdminMenu';
import { useCommonMenu } from '../useCommonMenu';
import { useSellerMenu } from '../useSellerMenu';

type Item =
  | MenuItem
  | {
      icon?: never;
      id: string;
      sup?: never;
      title: string;
      to?: never;
    };

type UseItems = () => Item[];

const useItems: UseItems = () => {
  const commonItems = useCommonMenu();
  const sellerItems = useSellerMenu();
  const adminItems = useAdminMenu();

  return useMemo(() => {
    return [...commonItems, ...sellerItems, ...adminItems];
  }, [adminItems, commonItems, sellerItems]);
};

export { useItems };
