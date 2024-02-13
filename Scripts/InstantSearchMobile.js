!(function (h) {
  h(document).ready(function () {
    var n,
      t,
      l = h(".instantSearchResourceElement");
    0 !== l.length &&
      ((0 < h("#instant-search-categories").length || 0 < h("#instant-search-manufacturers").length || 0 < h("#instant-search-vendors").length) &&
        (h(".search-box-text").addClass("narrow"), h(".store-search-box").addClass("with-caregory-search-enabled")),
        (n = parseInt(l.attr("data-numberOfVisibleProducts"))),
        (t = h(".instantSearchResourceElement").attr("data-noResultsResourceText")),
        $("#small-searchterms").kendoAutoComplete({
          highlightFirst: "true" === l.attr("data-highlightFirstFoundElement"),
          minLength: parseInt(l.attr("data-minKeywordLength")) || 0,
          enforceMinLength: !0,
          dataTextField: "ProductName",
          filter: "contains",
          noDataTemplate: t,
          popup: { appendTo: h(".instant-search-item") },
          template: kendo.template(h("#instantSearchItemTemplate").html()),
          height: "auto",
          dataSource: new kendo.data.DataSource({
            serverFiltering: !0,
            schema: { data: "Products" },
            requestStart: function () {
              h("#small-searchterms").addClass("instant-search-busy");
              console.log("Request Start: Searching...");
            },
            change: function () {
              h("#small-searchterms").removeClass("instant-search-busy");
              console.log("Change: Search Completed.");
            },
            transport: {
              read: {
                url: l.attr("data-instantSearchUrl"),
                complete: function (t) {
                  console.log("Read: Ajax Request Completed.");
                  t.success(function (t) {
                    var a = h("#instantSearchShowAll");
                    0 < a.length && a.remove(),
                      h("#small-searchterms-list").append(t.ShowAllButtonHtml),
                      h("#instantSearchShowAll").on("click", function (t) {
                        t.preventDefault(), h(".search-box-button").click();
                      });
                  });
                },
              },
              parameterMap: function () {
                console.log("Parameter Mapping: Mapping Parameters...");
                return { q: h("#small-searchterms").val(), categoryId: h("#instant-search-categories").val(), manufacturerId: h("#instant-search-manufacturers").val(), vendorId: h("#instant-search-vendors").val() };
              },
            },
          }),
          change: function () {
            var t = h(".k-list").find(".k-state-selected").find(".instant-search-item").attr("data-url");
            void 0 === t || setLocation(t);
            console.log("AutoComplete Change: Selected Item Changed.");
          },
          dataBound: function (t) {
            h("#small-searchterms").data("kendoAutoComplete").list.parent(".k-animation-container").addClass("instantSearch"),
              (0 < h("#instant-search-categories").length || 0 < h("#instant-search-manufacturers").length || 0 < h("#instant-search-vendors").length) && h(".k-animation-container").addClass("resize"),
              console.log("Data Bound: AutoComplete Data Bound.");
            (function () {
              var t = h("#small-searchterms_listbox").children().slice(0, n),
                a = 0;
              if (t.length === n) for (var e = 0; e < t.length; e++) a += h(t[e]).outerHeight();
              0 < a && h("#small-searchterms-list").css("height", a);
              console.log("Listbox Height Calculated: " + a);
            })();
          },
        }),

        "false" === l.attr("data-highlightFirstFoundElement") &&
        h("#small-search-box-form").on("keydown", function (t) {
          13 === t.keyCode && h(this).submit();
        }),
        h("#small-search-box-form").submit(function (t) {
          var a,
            e,
            n,
            r,
            s = h("#instant-search-categories").val() || 0,
            c = h("#instant-search-manufacturers").val() || 0,
            o = h("#instant-search-vendors").val() || 0,
            i = h("#small-searchterms").val();
          i &&
            ((a = l.attr("data-searchInProductDescriptions")),
              (e = 0 < s || 0 < c || 0 < o || a),
              (n = l.attr("data-searchPageUrl")),
              (r = encodeURIComponent(i)),
              (i = l.attr("data-defaultProductSortOption")),
              (window.location.href = n + "?advs=" + e + "&cid=" + s + "&mid=" + c + "&vid=" + o + "&q=" + r + "&sid=" + a + "&isc=true&orderBy=" + i)),
            t.preventDefault();
        }));
  });
})(jQuery);
