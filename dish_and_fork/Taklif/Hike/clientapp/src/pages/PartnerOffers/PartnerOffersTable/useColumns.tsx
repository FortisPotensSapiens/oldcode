import { format } from 'date-fns';
import { useMemo } from 'react';

import { ShowMoreTextTextField } from '~/components';
import { useCurrencySymbol } from '~/hooks';

import { ActionCell } from './ActionCell';
import { ApplicationCell } from './ApplicationCell';

const useColumns = () => {
  const currency = useCurrencySymbol();

  return useMemo(() => {
    const app = {
      field: 'applictaion',
      flex: 1,
      headerName: 'Заявка',
      renderCell: ApplicationCell,
      sortable: false,
    };

    return [
      app,

      {
        field: 'sum',
        flex: 1,
        headerName: 'Сумма',
        sortable: false,
        valueFormatter: ({ value }: any) => `${value} ${currency}`,
      },

      {
        field: 'date',
        flex: 1,
        headerName: 'Дата готовности',
        sortable: false,
        valueFormatter: ({ value }: any) => format(new Date(value), 'dd.MM.yyyy'),
      },

      {
        field: 'description',
        flex: 1,
        headerName: 'Описание',
        sortable: false,
        renderCell: ({ row }: { row: any }) => {
          return <ShowMoreTextTextField>{row.description}</ShowMoreTextTextField>;
        },
        valueFormatter: ({ value }: any) => value,
      },

      {
        field: 'actions',
        flex: 0,
        headerName: 'Действия',
        renderCell: ActionCell,
        renderHeader: () => null,
        sortable: false,
        width: 130,
      },
    ];
  }, [currency]);
};

export { useColumns };
