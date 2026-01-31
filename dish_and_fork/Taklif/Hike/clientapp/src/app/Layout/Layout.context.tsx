import React from 'react';

export type LayoutContextType = {
  showTestLabel: () => void;
  hideTestLabel: () => void;
};

const LayoutContext = React.createContext<LayoutContextType>({
  showTestLabel: () => {
    //
  },
  hideTestLabel: () => {
    //
  },
});

export { LayoutContext };
