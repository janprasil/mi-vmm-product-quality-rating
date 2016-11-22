import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { fetchContours, deleteAll } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  contours: state.api.getIn(['contours', 'data']),
  contoursError: state.api.getIn(['contours', 'error']),
  contoursPending: state.api.getIn(['contours', 'pending']),
  dtw: state.api.getIn(['dtw', 'data']),
  dtwError: state.api.getIn(['dtw', 'error']),
  dtwPending: state.api.getIn(['dtw', 'pending']),
  uploadPending: state.app.get('uploadPending'),
}), { fetchContours, deleteAll })
export default class Page extends Component {

  static propTypes = {
    contours: RPT.array,
    contoursError: RPT.bool,
    contoursPending: RPT.bool,
    deleteAll: RPT.func,
    dtw: RPT.array,
    dtwError: RPT.bool,
    dtwPending: RPT.bool,
    fetchContours: RPT.func.isRequired,
    uploadPending: RPT.bool
  }

  componentDidMount() {
    const { fetchContours } = this.props;
    // fetchContours();
  }

  renderContour() {
    const { contours } = this.props;
    if (!contours) return null;

    const data = contours.reduce((prev, x, idx) =>
      [...prev, {
        label: `chart${idx}`,
        values: Object.keys(x.timeline).reduce((prev2, key) =>
          [...prev2, { x: parseInt(key, 10), y: x.timeline[key] }]
        , [])
      }]
    , []);

    return (
      <LineChart
        data={data}
        width={600}
        height={200}
      />
    );
  }

  renderDtw() {
    const { dtw } = this.props;
    if (!dtw) return null;

    const data = [{
      label: 'name',
      values: dtw.reduce((prev, val) =>
          [...prev, { x: parseInt(val.split(', ')[0], 10), y: parseInt(val.split(', ')[1], 10) }]
        , [])
    }];

    return (
      <LineChart
        data={data}
        width={600}
        height={200}
      />
    );
  }

  render() {
    const { contoursError, contoursPending, deleteAll, uploadPending } = this.props;

    return (
      <div>
        <div>
          <button onClick={() => deleteAll()}>Smazat vše</button>
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg', 'png']}
            data={{ type: 'picture' }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/contours"
          >
            <button>
              Klikněte nebo přetáhněte JPG soubory
            </button>
          </FileUpload>
        </div>
        {contoursError && <p>Došlo k chybě. Určitě běží API?</p>}
        {uploadPending && <p>Odesílám fotky...</p>}
        {contoursPending && <p>Stahuji data a zpracovávám grafy...</p>}
        {this.renderContour()}
        {this.renderDtw()}
      </div>
    );
  }
}
