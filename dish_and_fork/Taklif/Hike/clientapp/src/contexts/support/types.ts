import { Dispatch, SetStateAction } from 'react';

export type SupportActionsContext = {
  change: Dispatch<SetStateAction<boolean>>;
  hide: () => void;
  show: () => void;
};

export type SupportContext = [boolean, SupportActionsContext];
