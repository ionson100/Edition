<script type="text/javascript">
function df(){
        $(function () {
            $("##acc#").accordion({
                autoHeight: true,
                active: #FirstOpenTab#
            });
        });
        };
        $(document).ready(df);
        try{
        $(document).ready(Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(df));
        }catch(e){
        }
    </script>