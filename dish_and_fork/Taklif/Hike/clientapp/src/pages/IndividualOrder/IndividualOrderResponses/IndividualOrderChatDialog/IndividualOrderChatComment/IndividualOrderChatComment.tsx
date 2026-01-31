import { styled, Typography } from '@mui/material';
import Grid from '@mui/material/Grid';
import dayjs from 'dayjs';
import LocalizedFormat from 'dayjs/plugin/localizedFormat';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';

import Avatar from '~/components/Avatar/Avatar';
import { OfferCommentReadModel } from '~/types';

dayjs.extend(LocalizedFormat);
dayjs.extend(utc);
dayjs.extend(timezone);

const StyledName = styled(Typography)`
  opacity: 0.6;
`;

const StyledDate = styled(Typography)`
  opacity: 0.4;
`;

const IndividualOrderChatComment = ({ comment }: { comment: OfferCommentReadModel }) => {
  return (
    <Grid container paddingBottom={2}>
      <Grid item paddingRight={2}>
        <Avatar userName={comment.userProfile.userName ?? ''} />
      </Grid>
      <Grid item xs>
        <Grid container>
          <Grid item xs sm>
            <StyledName variant="body2">{comment.userProfile.userName}</StyledName>
          </Grid>
          <Grid item>
            <StyledDate variant="body2">{dayjs(comment.created).local().format('LLL')}</StyledDate>
          </Grid>
          <Grid item xs={12}>
            <Typography variant="body2">{comment.text}</Typography>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default IndividualOrderChatComment;
