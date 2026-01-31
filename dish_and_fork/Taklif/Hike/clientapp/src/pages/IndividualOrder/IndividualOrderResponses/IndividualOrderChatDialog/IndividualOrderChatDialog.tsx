import { Close } from '@mui/icons-material';
import { Box, IconButton } from '@mui/material';
import { useMemo } from 'react';

import IndividualOrderChat from './IndividualOrderChat/IndividualOrderChat';
import { StyledDialog, StyledDialogContent, StyledHeader, StyledTitle } from './styled';

const IndividualOrderChatDialog = ({
  onHide,
  offerId,
  title,
}: {
  title: string;
  offerId: string | null;
  onHide: () => void;
}) => {
  const visible = useMemo(() => {
    return !!offerId;
  }, [offerId]);

  return (
    <StyledDialog
      onClose={onHide}
      open={visible}
      PaperProps={{
        sx: {
          borderRadius: { sm: 0, xs: 3 },
          height: { sm: '100%', xs: '90%' },
          marginBottom: { xs: 0 },
          marginLeft: { xs: 0 },
          marginRight: 0,
          marginTop: 0,
          maxHeight: { sm: '100%', xs: '90%' },
          width: { sm: '60%', xs: '100%' },
        },
      }}
    >
      <StyledDialogContent>
        <StyledHeader>
          <Box component={IconButton} onClick={onHide}>
            <Close />
          </Box>
          <Box paddingLeft={2}>
            Отклик на индивидуальный заказ: <StyledTitle>{title}</StyledTitle>
          </Box>
        </StyledHeader>

        <IndividualOrderChat offerId={String(offerId)} />
      </StyledDialogContent>
    </StyledDialog>
  );
};

export { IndividualOrderChatDialog };
