import { ComponentType } from 'react';

export type MenuItem = {
  icon?: ComponentType;
  id: string;
  sup?: string;
  title: string;
  to?: string;
  target?: string;
  external?: boolean;
};
